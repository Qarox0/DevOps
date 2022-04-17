using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Alchemy Recipe", menuName = "Scriptable/New Alchemy Recipe")]
public class AlchemyRecipeObject : ScriptableObject
{
    public                  GameObject OutComePrefab;
    public int        Amount1;
    public GameObject ItemPrefab1;
    [Space]
    public int Amount2;
    public GameObject ItemPrefab2;
    [Space]
    [SerializeField] public int Amount3;
    [SerializeField] public GameObject ItemPrefab3;
    [Space]
    [SerializeField] public int OutputAmount;
    [SerializeField] public GameObject OutputPrefab;
    
}
