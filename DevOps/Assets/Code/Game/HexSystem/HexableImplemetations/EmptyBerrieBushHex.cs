using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBerrieBushHex : MonoBehaviour, IHexable
{
    [SerializeField] private GameObject _stickPrefab;
    [SerializeField] private GameObject _berriesBushPrefab;
    [SerializeField] private int        _timeTaken        = 5;
    [SerializeField] private int        _maxUseCount      = 3;
    [SerializeField] private int        _timeNeededToGrow = 45;
    [SerializeField] private string     _prefabName       = "";
    
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
        TimeManager.GetTimeManagerInstance().onTimePasses += Grow;
        FieldType                                         =  HexType.BUSH;
        IsPassable                                        =  true;
        IsLaunchingOnEnter                                =  false;
        MovementMultiplier                                =  2;
        UseCount                                          =  0;
        GrowTime                                          =  0;
        Catched                                           =  default(CatchEnum);
        IsCatched                                         =  false;
        BaitInSlot                                        =  default(RequiredItem);
        TrapInSlot                                        =  default(RequiredItem);
        PrefabName                                        =  _prefabName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interaction(Player player)
    {
        if (UseCount < _maxUseCount)
        {
            UseCount++;
            Inventory.GetInventoryInstance().AddItemToInventory(_stickPrefab);
            TimeManager.GetTimeManagerInstance().PassTime(_timeTaken);
            SFXManager.GetInstance().Play("CollectSticks");
        }
        else
        {
            Depleted();
        }
    }

    public void Depleted()
    {
        
        Destroy(gameObject);
    }

    private void Grow(int time)
    {
        GrowTime += time;
        if (GrowTime >= _timeNeededToGrow)
        {
            var newBush = Instantiate(_berriesBushPrefab, transform.parent);
            transform.parent.GetComponent<HexScript>().SetObjectOnField(newBush);
            newBush.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (TimeManager.GetTimeManagerInstance() != null)
        {
            TimeManager.GetTimeManagerInstance().onTimePasses -= Grow;
        }
    }
}
