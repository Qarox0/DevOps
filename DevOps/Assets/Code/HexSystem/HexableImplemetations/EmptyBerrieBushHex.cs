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
    private                  int        _useCount         = 0;
    private                  int        _growTime         = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        TimeManager.GetTimeManagerInstance().onTimePasses += Grow;
        FieldType                                         =  HexType.BUSH;
        IsPassable                                        =  true;
        IsLaunchingOnEnter                                =  false;
        MovementMultiplier                                =  2;
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
        if (_useCount < _maxUseCount)
        {
            _useCount++;
            Inventory.GetInventoryInstance().AddItemToInventory(_stickPrefab);
            TimeManager.GetTimeManagerInstance().PassTime(_timeTaken);
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
        _growTime += time;
        if (_growTime >= _timeNeededToGrow)
        {
            var newBush = Instantiate(_berriesBushPrefab, transform.parent);
            transform.parent.GetComponent<HexScript>().SetObjectOnField(newBush);
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
