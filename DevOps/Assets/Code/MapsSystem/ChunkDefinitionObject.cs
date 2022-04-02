using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Chunk Definition", menuName = "Scriptable/Map/New Chunk Definition")]
public class ChunkDefinitionObject : ScriptableObject
{
    public GameObject ChunkPrefab;
    public int        ChanceToSpawn;
    public int        SizeX;
    public int        SizeY;
}
