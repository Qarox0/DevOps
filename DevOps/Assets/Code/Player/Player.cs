using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Player : MonoBehaviour, ISaveable
{
    public event Action onPlayerMove;

    public PlayerStats stats;
    
    [SerializeField] private GameObject     _inventoryHandle; //uchwyt do ui inv
    [SerializeField] private GameObject     _craftingHandle;  //uchwyt do ui inv
    [SerializeField] private GameObject     _buildingHandle;  //uchwyt do ui inv
    [SerializeField] private GameObject     _blocker;         //uchwyt do ui inv
    [SerializeField] private int            _timeTakenToMove; //Czas potrzebny na przejscie pola
    private                  SpriteRenderer _renderer;
    private                  int            _stepCounter = 0;
    private void Start()
    {
        InitializeStats();
        _renderer    =  GetComponent<SpriteRenderer>();
        onPlayerMove += HandleMoveEvents;
    }
    

    private void InitializeStats()
    {
        stats              = new PlayerStats();
        stats.MaxHunger    = 100;
        stats.MaxThirst    = 100;
        stats.Hunger       = 100;
        stats.Thirsty      = 100;
        stats.ActualHealth = stats.Health = 100;
        stats.Luck         = 1;
        stats.Sanity       = stats.HeadDamage = stats.TorsoDamage = stats.LeftLegDamage = stats.RightLegDamage = 100;
        stats.FatalRisk    = 1;
    }

    private void HandleMoveEvents()
    {
        if (_stepCounter == 100)
        {
            EventManager.GetInstance().LaunchEvent("PlanetPioneerEvent");
        }
    }

    public void InteractWithHexBelow(InputAction.CallbackContext value) //input interakcji z hexem na którym stoimy
    {
        if (value.started && _blocker.activeSelf == false)
        {
            transform.parent.GetComponent<HexScript>().HandlePlayerInteraction(this);
        }
    }

    public void Fishing(InputAction.CallbackContext value)
    {
        if (value.started && _blocker.activeSelf == false)
        {
            var fishingSpot = transform.parent.GetComponent<HexScript>().GetFishingSpot();
            if (fishingSpot != null)
            {
                fishingSpot.GetComponentInChildren<FishableHex>().Interaction(this);
            }
        }
    }

    public void MovePlayer(InputAction.CallbackContext value)               //input poruszania się
    {
        if (value.started && _blocker.activeSelf == false)
        {
            var          mousePosition = Mouse.current.position.ReadValue();
            Vector2      inWorldSpace  = Camera.main.ScreenToWorldPoint(mousePosition);
            RaycastHit2D hit           = Physics2D.Raycast(inWorldSpace, Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "Hex" && hit.collider.gameObject != transform.parent.gameObject)
            {
                var hex         = transform.parent.GetComponent<HexScript>();
                var objectOnHex = hit.collider.GetComponentInChildren<IHexable>();
                if (transform.parent.GetComponent<HexScript>().IsAdjecent(hit.collider.gameObject))
                {
                    if (objectOnHex != null && objectOnHex.IsPassable)
                    {
                        transform.SetParent(hit.collider.transform, false);
                        transform.parent.GetComponent<HexScript>().HandlePlayerEnter(this);
                        _renderer.sortingOrder = hit.collider.GetComponent<SpriteRenderer>().sortingOrder+1;
                        TimeManager.GetTimeManagerInstance().PassTime(_timeTakenToMove * objectOnHex.MovementMultiplier * hex.MovementMultiplier);
                        onPlayerMove?.Invoke();
                        _stepCounter++;
                    }
                    else if (objectOnHex == null)
                    {
                        _renderer.sortingOrder = hit.collider.GetComponent<SpriteRenderer>().sortingOrder+1;
                        transform.SetParent(hit.collider.transform, false);
                        transform.parent.GetComponent<HexScript>()?.HandlePlayerEnter(this);
                        TimeManager.GetTimeManagerInstance().PassTime(_timeTakenToMove * hex.MovementMultiplier);
                        onPlayerMove?.Invoke();
                        _stepCounter++;
                    }
                }
            }
        }
    }

    public void ToggleInventory(InputAction.CallbackContext value)                   //przełączanie ekwipunku
    {
        if (value.started)
        {
            _inventoryHandle.SetActive(!_inventoryHandle.activeSelf);
            SFXManager.GetInstance().Play("EQ");
        }
    }
    public void ToggleCrafting(InputAction.CallbackContext value) //przełączanie ekwipunku
    {
        if (value.started)
        {
            _craftingHandle.SetActive(!_craftingHandle.activeSelf);
            SFXManager.GetInstance().Play("EQ");
        }
    }
    
    public void ToggleBuilding(InputAction.CallbackContext value) //przełączanie ekwipunku
    {
        if (value.started)
        {
            _buildingHandle.SetActive(!_buildingHandle.activeSelf);
            SFXManager.GetInstance().Play("EQ");
        }
    }

    public void LaunchDebugEvent(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            EventManager.GetInstance().LaunchEvent("DebugEvent");
        }
    }

    public object CaptureState()
    {
        return new PlayerSaveData
        {
            HexName = transform.parent.name,
            Stats = stats,
            StepCount = _stepCounter
        };
    }

    public void   RestoreState(object state)
    {
        var data = (PlayerSaveData) state;
        transform.SetParent(GameObject.Find(data.HexName).transform, false);
        stats        = data.Stats;
        _stepCounter = data.StepCount;
    }
    [Serializable]
    public struct PlayerSaveData
    {
        public string      HexName;
        public int         StepCount;
        public PlayerStats Stats;
    }
}
