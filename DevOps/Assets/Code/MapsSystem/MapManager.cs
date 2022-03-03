using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Utils;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Transform           _generateParent;
    [SerializeField] private int                 _width        = 256;
    [SerializeField] private int                 _height       = 256;
    [SerializeField] private float               _waterScale   = 5f;
    [SerializeField] private float               _terrainScale = 20f;
    private                  string              _seed         = "";
    private                  float               _offsetX      = 0;
    private                  float               _offsetY      = 0;
    [SerializeField] private float               _hexWidth     = 1.74f;
    [SerializeField] private float               _hexHeight    = 1.74f;
    [SerializeField] private float               _hexOffsetY   = 1.74f;
    [SerializeField] private float               _tolerance    = 0.0001f;
    private                  MapDefinitionObject _mapDefinition;
    private                  Texture2D           _generatedWaterTexture;
    private                  Texture2D           _generatedTerrainTexture;
    private                  Transform           _returnPosition;

    private bool[,]       GridFillList;
    private GameObject[,] GridList;

    private static MapManager _Instance;

    public static MapManager GetInstance()
    {
        if (_Instance == null) _Instance = FindObjectOfType<MapManager>(); 
        return _Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    public void LoadLevelFromCurrentData(GameObject player, MapDefinitionObject mapDefinition)
    {
        while (_generateParent.childCount > 0)
        {
            Destroy(_generateParent.GetChild(0));
        }
        GenerateLevel(mapDefinition);
        _returnPosition = player.transform.parent;
        player.transform.SetParent(FindStartingPos(), false);
    }
    
    private void GenerateLevel(MapDefinitionObject mapDefinition)
    {
        GridFillList = new bool[_width, _height];
        GridList     = new GameObject[_width, _height];
        GenerateSeed();
        CalculateOffsetFromSeed();
        _generatedWaterTexture   = GenerateNoise(_waterScale, mapDefinition.WaterDiversities);
        _generatedTerrainTexture = GenerateNoise(_terrainScale, mapDefinition.TerrainDiversities);
        var parent = new GameObject("GridHolder");
        parent.transform.parent = _generateParent;
        GenerateHexMapFromTexture(parent);
    }

    private Transform FindStartingPos()
    {
        bool      found = false;
        Transform pos = null;
        while (found == false)
        {
            int direction = Random.Range(0, 3); //0- LEFT/1- TOP/2- Right/3- Bottom
            switch (direction)
            {
                case 0:
                    int y = Random.Range(0, GridList.GetLength(1)-1);
                    if (GridList[0, y].GetComponentInChildren<IHexable>().IsPassable)
                        pos= GridList[0, y].transform;
                    found = true;
                    break;
                case 1:
                    int x = Random.Range(0, GridList.GetLength(0) -1);
                    if (GridList[x, 0].GetComponentInChildren<IHexable>().IsPassable)
                        pos = GridList[0, x].transform;
                    found = true;
                    break;
                case 2:
                    int ye = Random.Range(0, GridList.GetLength(1) -1);
                    if (GridList[GridList.GetLength(0)        -1, ye].GetComponentInChildren<IHexable>().IsPassable)
                        pos = GridList[GridList.GetLength(0) -1, ye].transform;
                    found = true;
                    break;
                case 3:
                    int xe = Random.Range(0, GridList.GetLength(0) -1);
                    if (GridList[xe, GridList.GetLength(1) -1].GetComponentInChildren<IHexable>().IsPassable)
                        pos = GridList[xe, GridList.GetLength(1) -1].transform;
                    found = true;
                    break;
            }
        }

        return pos;
    }
    private Texture2D GenerateNoise(float scale, List<GenerateLevelDiversity> diversities)
    {
        Texture2D texture2D = new Texture2D(_width,_height);

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Color color = CalculateColor(x, y, scale,diversities);
                texture2D.SetPixel(x,y,color);
            }
        }
        texture2D.Apply();
        return texture2D;
    }
    private void GenerateSeed()
    {
        string randomSeed = "";
        foreach (var numberOfLetters in GlobalConsts.WordsWithNumberOfLetters)
        {
            for (int i = 0; i < Random.Range(1,numberOfLetters); i++)
            {
                randomSeed += GlobalConsts.ListOfLetters[Random.Range(0,GlobalConsts.ListOfLetters.Length)];
            }

            randomSeed += ' ';
        }

        _seed = randomSeed;
    }
    private void CalculateOffsetFromSeed()
    {
        string[] splittedSeed = _seed.Split(' ');
        float    x = 0,y = 0;
        if (splittedSeed.Length >= 3)
        {
            foreach (char seedchar in splittedSeed[0])
            {
                x += seedchar;
            }
            foreach (char seedchar in splittedSeed[1])
            {
                x += float.Parse($"0,{(int)seedchar}");
            }
            foreach (char seedchar in splittedSeed[2])
            {
                y += seedchar;
            }
            foreach (char seedchar in splittedSeed[3])
            {
                y += float.Parse($"0,{(int)seedchar}");
                
            }
        }

        _offsetX = x;
        _offsetY = y;
    }

    private Color CalculateColor(int x, int y, float scale, List<GenerateLevelDiversity> diversities)
    {
        float xCoord = (float)x /_width  * scale + _offsetX;
        float yCoord = (float)y /_height * scale + _offsetY;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        Color color;
        foreach (var diversity in diversities)
        {
            if (sample >= diversity.MinValue && sample <= diversity.MaxValue)
            {
                float roll = Random.Range(0, 100);
                if (diversity.ChanceOfObject >= roll)
                {
                    color              = new Color(diversity.MarkColor.r, diversity.MarkColor.g, diversity.MarkColor.b);
                    return color;
                }
            }
        }
        
        color              = new Color(sample, sample, sample);
        return color;
    }

    private void GenerateHexMapFromTexture(GameObject parent)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                
                SpawnFromDiversity(_generatedWaterTexture,   _mapDefinition.WaterDiversities,   x, y, parent);
                SpawnFromDiversity(_generatedTerrainTexture, _mapDefinition.TerrainDiversities, x, y, parent);
                if (GridFillList[x, y] == false)
                {
                    SpawnEmpty(_mapDefinition.DefaultFillPrefab, x, y, parent);
                }
                
            }
        }
    }

    private void SpawnEmpty(GameObject nullPrefab, int x, int y, GameObject parent)
    {
        var child         = Instantiate(nullPrefab, positionFromCoords(x, y), new Quaternion());
        child.transform.SetParent(parent.transform, true);
        child.name                                        = $"{nullPrefab.name}({x},{y})";
        child.GetComponent<SpriteRenderer>().sortingOrder = y - 1;
    }
    private void SpawnFromDiversity(Texture2D texture, List<GenerateLevelDiversity> diversities, int x, int y, GameObject parent)
    {
        if (GridFillList[x, y] == false)
        {
            foreach (var diversity in diversities)
            {
                
                if (Math.Abs(texture.GetPixel(x, y).r - diversity.MarkColor.r) < _tolerance &&
                    Math.Abs(texture.GetPixel(x, y).g - diversity.MarkColor.g) < _tolerance &&
                    Math.Abs(texture.GetPixel(x, y).b - diversity.MarkColor.b) < _tolerance)
                {
                    var objectToSpawn = diversity.FloraList[Random.Range(0, diversity.FloraList.Count)];
                    var child         = Instantiate(objectToSpawn, positionFromCoords(x, y), new Quaternion());
                    child.transform.SetParent(parent.transform, true);
                    child.name         = $"{objectToSpawn.name}({x},{y})";
                    GridFillList[x, y] = true;
                    GridList[x, y]     = child;
                    if (child.transform.childCount                                 > 0 &&
                        child.transform.GetChild(0).GetComponent<SpriteRenderer>() != null)
                        child.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = y;
                    child.GetComponent<SpriteRenderer>().sortingOrder = y - 1;
                }
            }
        }

    }

    private Vector2 positionFromCoords(int x, int y)
    {
        var vec =new Vector2();
        if (x % 2 == 0)
        {
            vec.x = x *_hexWidth;
        }
        else
        {
            vec.x = x *_hexWidth;
        }
        if (x % 2 == 0)
        {
            vec.y = -y*_hexHeight;
        }
        else
        {
            vec.y = -y *_hexHeight + _hexOffsetY;
        }

        return vec;
    }
}
