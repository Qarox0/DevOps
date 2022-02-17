using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class TrapHex : MonoBehaviour, IHexable
{
    public  RequiredItem BaitInSlot;
    public  RequiredItem TrapInSlot;
    private CatchEnum    _catched; //co zostało złapane w tego hexa
    private bool         _isCatched = false;

    void Start()
    {
        FieldType          = HexType.STATION;
        IsLaunchingOnEnter = false;
        IsPassable         = true;
        MovementMultiplier = 1;
    }

    public HexType FieldType          { get; set; }
    public bool    IsLaunchingOnEnter { get; set; }
    public bool    IsPassable         { get; set; }
    public int     MovementMultiplier { get; set; }
    public void    Interaction(Player player)
    {
        if(_isCatched)
            EventManager.GetInstance().LaunchEvent(_catched.EventName);
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
            baitCatch = BaitInSlot.ItemNeeded.GetComponent<Item>().GetCatch();
        }

        if (TrapInSlot.ItemNeeded != null)
        {
            trapCatch = TrapInSlot.ItemNeeded.GetComponent<Item>().GetCatch();
        }

        if (!baitCatch.Equals(default(CatchEnum)) && !trapCatch.Equals(default(CatchEnum)))
        {
            Random random = new Random();
            int    roll   = random.Next(0, 100);
            if (roll < 51)
            {
                _catched = baitCatch;
            }
            else
            {
                _catched = trapCatch;
            }
        } else if( !baitCatch.Equals(default(CatchEnum)))
        {
            _catched = baitCatch;
        } else if (!trapCatch.Equals(default(CatchEnum)))
        {
            _catched = trapCatch;
        }

        if (!_catched.Equals(default(CatchEnum)))
        {
            _isCatched = true;
        }
    }
}
