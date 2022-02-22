using System.Collections;
using System.Collections.Generic;
using Code.Utils;
using UnityEngine;
using Random = System.Random;

public class TrapHex : MonoBehaviour, IHexable
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

    private void Start()
    {
        FieldType                       =  HexType.STATION;
        IsLaunchingOnEnter              =  false;
        IsPassable                      =  true;
        MovementMultiplier              =  1;
        UseCount                        =  0;
        GrowTime                        =  0;
        Catched                         =  default(CatchEnum);
        IsCatched                       =  false;
        BaitInSlot                      =  default(RequiredItem);
        TrapInSlot                      =  default(RequiredItem);
        PrefabName                      =  _prefabName;
        SLAM.GetInstance().OnSaveLoaded += OnLoad;
    }

    void OnLoad()
    {
        TrapManager.GetInstance().startCatching(this);
    }

    public void    Interaction(Player player)
    {
        if(IsCatched)
            EventManager.GetInstance().LaunchEvent(Catched.EventName);
        else
        {
            TrapManager.GetInstance().ToggleTrapPanel(this);
        }

    }

    public void    Depleted()
    {
        Debug.LogError("Not implemented yet");
    }

    public void TryCatch(int time)
    {
        CatchEnum baitCatch = default(CatchEnum);
        CatchEnum trapCatch = default(CatchEnum);
        if (BaitInSlot.ItemNeeded != null)
        {
            baitCatch = Resources.Load<GameObject>(GlobalConsts.PathToItems +BaitInSlot.ItemNeeded).GetComponent<Item>().GetCatch();
        }

        if (TrapInSlot.ItemNeeded != null)
        {
            trapCatch = Resources.Load<GameObject>(GlobalConsts.PathToItems +TrapInSlot.ItemNeeded).GetComponent<Item>().GetCatch();
        }

        if (!baitCatch.Equals(default(CatchEnum)) && !trapCatch.Equals(default(CatchEnum)))
        {
            Random random = new Random();
            int    roll   = random.Next(0, 100);
            if (roll < 51)
            {
                Catched = baitCatch;
            }
            else
            {
                Catched = trapCatch;
            }
        } else if( !baitCatch.Equals(default(CatchEnum)))
        {
            Catched = baitCatch;
        } else if (!trapCatch.Equals(default(CatchEnum)))
        {
            Catched = trapCatch;
        }

        if (!Catched.Equals(default(CatchEnum)))
        {
            IsCatched = true;
        }
        
    }
}
