using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickHex : MonoBehaviour, IHexable
{
    [SerializeField] private GameObject _stickInvPrefab;
    [SerializeField] private int        _timeTaken;
    [SerializeField] private int        _maxUseCount;

    private int _useCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        //ustawianie właściwości
        FieldType          = HexType.BUSH;
        IsLaunchingOnEnter = false;
        IsPassable         = true;
        MovementMultiplier = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public HexType FieldType          { get; set; }
    public bool    IsLaunchingOnEnter { get; set; }
    public bool    IsPassable         { get; set; }
    public int     MovementMultiplier { get; set; }

    public void Interaction(Player player)
    {
        Inventory.GetInventoryInstance().AddItemToInventory(_stickInvPrefab);
        TimeManager.GetTimeManagerInstance().PassTime(_timeTaken);
        SFXManager.GetInstance().Play("CollectSticks");
        _useCount++;
        Depleted();
    }

    public void Depleted()
    {
        if (_useCount == _maxUseCount)
        {
            Destroy(this.gameObject);
        }
    }
}
