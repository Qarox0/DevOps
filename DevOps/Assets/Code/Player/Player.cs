using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public event Action onPlayerMove;

    public PlayerStats stats;
    
    [SerializeField] private GameObject _inventoryHandle; //uchwyt do ui inv
    [SerializeField] private GameObject _craftingHandle;  //uchwyt do ui inv
    [SerializeField] private GameObject _buildingHandle;  //uchwyt do ui inv
    [SerializeField] private int        _timeTakenToMove; //Czas potrzebny na przejscie pola

    private void Start()
    {
        InitializeStats();
    }
    

    private void InitializeStats()
    {
        stats              = new PlayerStats();
        stats.ActualHealth = stats.Health = 100;
        stats.Luck         = 1;
        stats.Sanity       = stats.HeadDamage = stats.TorsoDamage = stats.LeftLegDamage = stats.RightLegDamage = 100;
        stats.FatalRisk    = 1;
    }

    public void InteractWithHexBelow(InputAction.CallbackContext value) //input interakcji z hexem na którym stoimy
    {
        if (value.started)
        {
            transform.parent.GetComponent<HexScript>().HandlePlayerInteraction(this);
        }
    }

    public void Fishing(InputAction.CallbackContext value)
    {
        if (value.started)
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
        if (value.started)
        {
            var          mousePosition = Mouse.current.position.ReadValue();
            Vector2      inWorldSpace  = Camera.main.ScreenToWorldPoint(mousePosition);
            RaycastHit2D hit           = Physics2D.Raycast(inWorldSpace, Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "Hex")
            {
                var hex         = transform.parent.GetComponent<HexScript>();
                var objectOnHex = hit.collider.GetComponentInChildren<IHexable>();
                if (transform.parent.GetComponent<HexScript>().IsAdjecent(hit.collider.gameObject))
                {
                    if (objectOnHex != null && objectOnHex.IsPassable)
                    {
                        transform.SetParent(hit.collider.transform, false);
                        TimeManager.GetTimeManagerInstance().PassTime(_timeTakenToMove * objectOnHex.MovementMultiplier * hex.MovementMultiplier);
                        onPlayerMove?.Invoke();
                    }
                    else if (objectOnHex == null)
                    {
                        transform.SetParent(hit.collider.transform, false);
                        TimeManager.GetTimeManagerInstance().PassTime(_timeTakenToMove * hex.MovementMultiplier);
                        onPlayerMove?.Invoke();
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
        }
    }
    public void ToggleCrafting(InputAction.CallbackContext value) //przełączanie ekwipunku
    {
        if (value.started)
        {
            _craftingHandle.SetActive(!_craftingHandle.activeSelf);
        }
    }
    
    public void ToggleBuilding(InputAction.CallbackContext value) //przełączanie ekwipunku
    {
        if (value.started)
        {
            _buildingHandle.SetActive(!_buildingHandle.activeSelf);
        }
    }

    public void LaunchDebugEvent(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            EventManager.GetInstance().LaunchEvent("DebugEvent");
        }
    }

}
