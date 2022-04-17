using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcheologyHex : MonoBehaviour, IHexable
{
    [SerializeField] private ArcheologyDataObject _data;
    // Start is called before the first frame update
    void Start()
    {
        UseCount           = 0;
        MovementMultiplier = 1;
        IsPassable         = true;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        ArcheologyManager.GetInstance().OpenArcheology(_data);
        UseCount++;
        if (UseCount > 0)
        {
            Depleted();
        }
    }

    public void         Depleted()
    {
        Destroy(gameObject);
    }
}
