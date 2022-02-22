using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeHex : MonoBehaviour, IHexable
{
    //Pola Inspektora
    [Header("Costumizable Fields")]
    [Tooltip("How many times this field can be used")]
    [SerializeField] private int        _maxUseCount = 5;        //Ile Razy pole może zostać użyte
    [Tooltip("how much time is taken from action")]
    [SerializeField] private int        _timeTaken = 5;          //Ile czasu ma być zabrane za akcje w minutach
    [Tooltip("Prefab with wood, which will go to inv")]
    [SerializeField] private GameObject _woodInvPrefab;          //Prefab drewna, które trafia do ekwipunku
    [Tooltip("Item required to interact")]
    [SerializeField] private Item       _requiredItemToInteract; //Item potrzebny do interakcji
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
    private void Start()
    {
        //ustawianie właściwości
        FieldType          = HexType.TREE;
        IsLaunchingOnEnter = false;
        IsPassable         = true;
        MovementMultiplier = 2;
        UseCount           = 0;
        GrowTime           = 0;
        Catched            = default(CatchEnum);
        IsCatched          = false;
        BaitInSlot         = default(RequiredItem);
        TrapInSlot         = default(RequiredItem);
        PrefabName         = _prefabName;
    }

    #region InheritedFromIHexable
    public void    Interaction(Player player)
    {
        if (Inventory.GetInventoryInstance().IsHaving(_requiredItemToInteract))
        {
            Inventory.GetInventoryInstance().AddItemToInventory(_woodInvPrefab);
            TimeManager.GetTimeManagerInstance().PassTime(_timeTaken);
            UseCount++;
            SFXManager.GetInstance().Play("Chop");
            Depleted();
        }
    }

    public void    Depleted()
    {
        if (UseCount == _maxUseCount)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

}
