using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa <c>SaveData</c> to kontener na dane do zapisu
/// jeśli chcemy coś zapisać używamy do tego właśnie tego obiektu
/// </summary>
[Serializable]
public class SaveData
{
    public float Health;
    public float ActualHealth;
    public float FatalRisk;
    public float Luck;
    public float Sanity;
    public float TorsoDamage;
    public float HeadDamage;
    public float LeftLegDamage;
    public float RightLegDamage;

    public List<string> InventoryItemsName;
    public List<int>    InventoryItemsQuantity;

}
