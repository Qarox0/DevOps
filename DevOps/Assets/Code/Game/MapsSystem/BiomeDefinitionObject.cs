using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Biome Definition Object", menuName = "Scriptable/Map/New Biome Definition Object")]
public class BiomeDefinitionObject : ScriptableObject
{
    public List<GenerateLevelDiversity> TerrainDiversities;
    public List<GenerateLevelDiversity> WaterDiversities;
    public List<ChunkDefinitionObject>  UniqueChunks;
    public Biomes                       BiomeType;
    public Flora                        FloraType;
    public GameObject                   DefaultFillPrefab;
}
