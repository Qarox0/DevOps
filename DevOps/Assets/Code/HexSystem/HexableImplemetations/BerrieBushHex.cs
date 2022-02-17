using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerrieBushHex : MonoBehaviour, IHexable
{
    [SerializeField] private GameObject _berriesPrefab;
    [SerializeField] private GameObject _emptyBushPrefab;
    [SerializeField] private int        _maxUseCount = 10;
    [SerializeField] private int        _timeTaken   = 3;
    private                  int        _useCount    = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        FieldType          = HexType.BUSH;
        IsPassable         = true;
        IsLaunchingOnEnter = false;
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
    public void    Interaction(Player player)
    {
        if (_useCount < _maxUseCount)
        {
            _useCount++;
            Inventory.GetInventoryInstance().AddItemToInventory(_berriesPrefab);
            TimeManager.GetTimeManagerInstance().PassTime(_timeTaken);
        }
        else
        {
            Depleted();
        }
    }

    public void    Depleted()
    {
        var newBush =Instantiate(_emptyBushPrefab, transform.parent);
        transform.parent.GetComponent<HexScript>().SetObjectOnField(newBush);
        Destroy(gameObject);
    }
}
