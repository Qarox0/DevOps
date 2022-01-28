using UnityEngine;

public class CampfireHex : MonoBehaviour, IHexable
{
    // Start is called before the first frame update
    private int                 _timePassed;
    public  RequiredItem        FirstSlotItem  { get; set; }
    public  RequiredItem        SecondSlotItem { get; set; }
    public  RequiredItem        ThirdSlotItem  { get; set; }
    public  CookingRecipeObject Recipe         { get; set; }
    public  RequiredItem        Output         { get; set; }
    private GameObject          _fuelPrefab;
    public  int                 FuelAmount { get; set; }
    public  bool                IsCooking  { get; private set; }
    private int                 _fuelBurningTime = 0;
    private Transform           _garbageDest;
    private void SetFuel(GameObject fuel)
    {
        if(fuel != null)
            _fuelPrefab = Instantiate(fuel, _garbageDest, false);
    }
    void Start()
    {
        FieldType          = HexType.STATION;
        IsLaunchingOnEnter = false;
        IsPassable         = true;
        MovementMultiplier = 1;
        _garbageDest       = GameObject.FindWithTag("Garbage").transform;   //TODO Refactor (Slaby performance, ulepszyc)
    }

    public void StartCooking(GameObject fuelPrefab)
    {
        _timePassed                                       =  0;
        TimeManager.GetTimeManagerInstance().onTimePasses += CalulateTime;
        Output                                            =  Recipe.RawOutput;
        FirstSlotItem                                     =  Recipe.ItemNeeded1;
        SecondSlotItem                                    =  Recipe.ItemNeeded2;
        ThirdSlotItem                                     =  Recipe.ItemNeeded3;
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
        _fuelBurningTime += burningTime;
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
        return _fuelPrefab;
    }

    private void CalulateTime(int time)
    {
        _timePassed += time;
        if (_timePassed < Recipe.MinimalTime)
        {
            Output = Recipe.RawOutput;
        }else if (_timePassed > Recipe.MinimalTime && _timePassed < Recipe.MaximalTime)
        {
            Output = Recipe.Output;
        }
        else
        {
            Output = Recipe.FailureOutput;
        }

        _fuelBurningTime -= time;
        if (_fuelPrefab != null && _fuelBurningTime <= 0)
        {
            if (FuelAmount <= 0)
            {
                SetFuel(null);
                StopCooking(false);
            }
            else
            {
                FuelAmount--;
                _fuelBurningTime += _fuelPrefab.GetComponent<Item>()._fuelValue;
            }
        }
    }
    
    public HexType FieldType          { get; set; }
    public bool    IsLaunchingOnEnter { get; set; }
    public bool    IsPassable         { get; set; }
    public int     MovementMultiplier { get; set; }
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
