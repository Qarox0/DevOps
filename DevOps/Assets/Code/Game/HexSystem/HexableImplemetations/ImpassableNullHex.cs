using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpassableNullHex : MonoBehaviour, IHexable
{
    [SerializeField] private string _prefabName = "";
    
    #region IHexable Fields

    //Pola właściwości
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

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        FieldType          = HexType.NULL;
        IsLaunchingOnEnter = false;
        IsPassable         = false;
        MovementMultiplier = 0;
        UseCount           = 0;
        GrowTime           = 0;
        Catched            = default(CatchEnum);
        IsCatched          = false;
        BaitInSlot         = default(RequiredItem);
        TrapInSlot         = default(RequiredItem);
        PrefabName         = _prefabName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void    Interaction(Player player)
    {
        Debug.LogError("this shouldn't be possible");
    }

    public void    Depleted()
    {
        Debug.LogError("this shouldn't be possible");

    }
}
