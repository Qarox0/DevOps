using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _hammerPrefab;    //Prefab młotka, póki nie ma craftingu i siekiery
    [SerializeField] private GameObject _inventoryHandle; //uchwyt do ui inv
    [SerializeField] private GameObject _craftingHandle;  //uchwyt do ui inv
    [SerializeField] private GameObject _buildingHandle;  //uchwyt do ui inv
    [SerializeField] private int        _timeTakenToMove; //Czas potrzebny na przejscie pola
    public void InteractWithHexBelow(InputAction.CallbackContext value) //input interakcji z hexem na którym stoimy
    {
        if (value.started)
        {
            transform.parent.GetComponent<HexScript>().HandlePlayerInteraction(this);
            //Debug.Log("interacted");
            /*if (!Inventory.GetInventoryInstance()
                          .IsHaving(_hammerPrefab
                                        .GetComponent<Item>())) //Debug - dodanie młotka, żeby móc ścinać drzewa
            {
                Inventory.GetInventoryInstance().AddItemToInventory(_hammerPrefab);
            }*/
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
                        
                    }
                    else if (objectOnHex == null)
                    {
                        transform.SetParent(hit.collider.transform, false);
                        TimeManager.GetTimeManagerInstance().PassTime(_timeTakenToMove * hex.MovementMultiplier);
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

}
