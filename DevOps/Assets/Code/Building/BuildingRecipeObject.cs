using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BuildingRecipe", menuName = "Scriptable/new Building Recipe")]
public class BuildingRecipeObject : ScriptableObject
{
    public List<RequiredItem> ItemsNeeded;
    public Sprite             BuildMenuImage;
    public GameObject         Output;
}

[Serializable]
public struct RequiredItem
{
    public GameObject ItemNeeded;
    public int        Amount;
}

