using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellHex : MonoBehaviour, IHexable
{
    [SerializeField] private GameObject _waterPrefab;
    [SerializeField] private GameObject _prefabName;
    // Start is called before the first frame update
    void Start()
    {
        FieldType          = HexType.BUSH;
        IsPassable         = true;
        IsLaunchingOnEnter = false;
        MovementMultiplier = 2;
        UseCount           = 0;
        GrowTime           = 0;
        Catched            = default(CatchEnum);
        IsCatched          = false;
        BaitInSlot         = default(RequiredItem);
        TrapInSlot         = default(RequiredItem);
        PrefabName         = _prefabName.name;
    }


    public string       PrefabName         { get; set; }
    public HexType      FieldType          { get; set; }
    public bool         IsLaunchingOnEnter { get; set; }
    public bool         IsPassable         { get; set; }
    public int          MovementMultiplier { get; set; }
    public int          UseCount           { get; set; }
    public int          GrowTime           { get; set; }
    public CatchEnum    Catched            { get; set; }
    public bool         IsCatched          { get; set; }
    public RequiredItem BaitInSlot         { get; set; }
    public RequiredItem TrapInSlot         { get; set; }
    public int          FuelBurningTime    { get; set; }
    public string       FuelPrefab         { get; set; }
    public int          TimePassed         { get; set; }
    public string       Recipe             { get; set; }
    public RequiredItem Output             { get; set; }
    public bool         IsCooking          { get; set; }
    public int          FuelAmount         { get; set; }
    public void         Interaction(Player player)
    {   
        Inventory.GetInventoryInstance().AddItemToInventory(_waterPrefab);
    }

    public void         Depleted()
    {
    }
}
