using System;
using Code.Utils;
using UnityEditor;
using UnityEngine;

public class CampfireHex : MonoBehaviour, IHexable
{
    [SerializeField] private string _prefabName;
    // Start is called before the first frame update
    public  RequiredItem        FirstSlotItem  { get; set; }
    public  RequiredItem        SecondSlotItem { get; set; }
    public  RequiredItem        ThirdSlotItem  { get; set; }
    private Transform           _garbageDest;
    
    #region IHexable Fields
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
    private void SetFuel(GameObject fuel)
    {
        if (fuel != null)
            FuelPrefab = fuel.GetComponent<Item>().PrefabName;
    }
    void Start()
    {
        FieldType          = HexType.STATION;
        IsLaunchingOnEnter = false;
        IsPassable         = true;
        MovementMultiplier = 1;
        _garbageDest       = GameObject.FindWithTag("Garbage").transform;   //TODO Refactor (Slaby performance, ulepszyc)
        PrefabName = _prefabName;
        SLAM.GetInstance().OnSaveLoaded += OnLoad;
    }

    void OnLoad()
    {
        if (IsCooking)
        {
            TimeManager.GetTimeManagerInstance().onTimePasses += CalulateTime;
        }
    }

    public void StartCooking(GameObject fuelPrefab)
    {
        var recipe = Resources.Load(GlobalConsts.PathToRecpies + Recipe) as CookingRecipeObject;
        TimePassed                                       =  0;
        TimeManager.GetTimeManagerInstance().onTimePasses += CalulateTime;
        Output                                            =  recipe.RawOutput;
        FirstSlotItem                                     =  recipe.ItemNeeded1;
        SecondSlotItem                                    =  recipe.ItemNeeded2;
        ThirdSlotItem                                     =  recipe.ItemNeeded3;
        IsCooking                                        =  true;
        if (fuelPrefab != null)
        {
            SetFuel(fuelPrefab);
        }
    }

    private void StopCooking(bool withOutput)
    {
        TimeManager.GetTimeManagerInstance().onTimePasses -= CalulateTime;
        if (withOutput)
        {
            Output = new RequiredItem();
        }
        FirstSlotItem  = new RequiredItem();
        SecondSlotItem = new RequiredItem();
        ThirdSlotItem  = new RequiredItem();
        Recipe         = null;
        IsCooking      = false;
    }

    public void AddBurningTime(int burningTime)
    {
        FuelBurningTime += burningTime;
    }

    public void OnClose(CookingManager manager)
    {
        
        if (Recipe == null)
        {
            TimeManager.GetTimeManagerInstance().onTimePasses -= CalulateTime;
        }

        if (manager.IsOutputClean())
        {
            StopCooking(true);
        }

        if (manager.IsFuelClean())
        {
            SetFuel(null);
        }
    }

    public GameObject GetFuelPrefab()
    {
        return Resources.Load<GameObject>(GlobalConsts.PathToItems+FuelPrefab);
    }

    private void CalulateTime(int time)
    {
        var recipe = Resources.Load(GlobalConsts.PathToRecpies + Recipe) as CookingRecipeObject;
        TimePassed += time;
        if (TimePassed < recipe.MinimalTime)
        {
            Output = recipe.RawOutput;
        }else if (TimePassed > recipe.MinimalTime && TimePassed < recipe.MaximalTime)
        {
            Output = recipe.Output;
        }
        else
        {
            Output = recipe.FailureOutput;
        }
        Debug.Log("Cooking");

        FuelBurningTime -= time;
        if (FuelPrefab != null && FuelBurningTime <= 0)
        {
            if (FuelAmount <= 0)
            {
                SetFuel(null);
                StopCooking(false);
            }
            else
            {
                FuelAmount--;
                FuelBurningTime += Resources.Load<GameObject>(GlobalConsts.PathToItems +FuelPrefab).GetComponent<Item>()._fuelValue;
            }
        }
    }
    
    public void    Interaction(Player player)
    {
        if (!CookingManager.GetInstance().IsOpen)
        {
            CookingManager.GetInstance().Init(this);
        }
    }

    public void    Depleted()
    {
    }

    private void OnDestroy()
    {
        if (TimeManager.GetTimeManagerInstance() != null)
        {
            TimeManager.GetTimeManagerInstance().onTimePasses -= CalulateTime;
        }
    }
}
