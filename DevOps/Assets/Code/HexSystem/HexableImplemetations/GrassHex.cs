using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassHex : MonoBehaviour, IHexable
{
    [SerializeField] private GameObject _grassInvPrefab;
    [SerializeField] private int        _timeTaken;
    [SerializeField] private int        _maxUseCount;

    private int _useCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        //ustawianie właściwości
        FieldType          = HexType.GRASS;
        IsLaunchingOnEnter = false;
        IsPassable         = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public HexType FieldType          { get; set; }
    public bool    IsLaunchingOnEnter { get; set; }
    public bool    IsPassable         { get; set; }
    public void    Interaction(Player player)
    {
        Inventory.GetInventoryInstance().AddItemToInventory(_grassInvPrefab);
        TimeManager.GetTimeManagerInstance().PassTime(_timeTaken);
        _useCount++;
        Depleted();
    }

    public void    Depleted()
    {
        if (_useCount == _maxUseCount)
        {
            Destroy(this.gameObject);
        }
    }
}
