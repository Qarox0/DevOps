using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishableHex : MonoBehaviour, IHexable
{
    [SerializeField] private List<GameObject> _fishesToCatch;
    [SerializeField] private GameObject       _nullHexPrefab;
    [SerializeField] private int              _maxUseCount;

    private int _useCount;
    
    // Start is called before the first frame update
    void Start()
    {
        FieldType          = HexType.FISHING;
        IsLaunchingOnEnter = false;
        IsPassable         = false;
        MovementMultiplier = 0;
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
        var fishCaught = Random.Range(0, _fishesToCatch.Count - 1);
        Inventory.GetInventoryInstance().AddItemToInventory(_fishesToCatch[fishCaught]);
        _useCount++;
        Depleted();
    }

    public void    Depleted()
    {
        if (_useCount >= _maxUseCount)
        {
            Instantiate(_nullHexPrefab, transform.parent);
            Destroy(gameObject);
        }
    }
}
