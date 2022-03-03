using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Map Definition Object", menuName = "Scriptable/Map Definition Object")]
public class MapDefinitionObject : ScriptableObject
{
    public List<GenerateLevelDiversity> TerrainDiversities;
    public List<GenerateLevelDiversity> WaterDiversities;
    public Biomes                       BiomeType;
    public Flora                        FloraType;
    public GameObject                   DefaultFillPrefab;
}
