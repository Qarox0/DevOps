using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SlotScript : MonoBehaviour, IDropHandler
{
    [SerializeField] private List<ItemType> AcceptedItems;

    [SerializeField] private bool _isEquipSlot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        var objectOnSlot  = GetComponentInChildren<Item>();
        var droppedObject = eventData.pointerDrag;
        if (objectOnSlot != null && droppedObject != null && objectOnSlot.PrefabName == droppedObject.GetComponent<Item>().PrefabName)
        {
            droppedObject.GetComponent<Item>().IsOnItem = true;
            droppedObject.GetComponent<Item>().IsDroppedOnSlot = true;
            if (Keyboard.current.altKey.isPressed && droppedObject.GetComponent<Item>().Quantity < droppedObject.GetComponent<Item>().GetMaxStackQuantity())
            {
                objectOnSlot.Quantity++;
                droppedObject.GetComponent<Item>().Quantity--;
                if (droppedObject.GetComponent<Item>().Quantity == 0)
                {
                    Destroy(droppedObject);
                }
                
            }
            else
            {
                if (droppedObject.GetComponent<Item>().Quantity <
                    droppedObject.GetComponent<Item>().GetMaxStackQuantity())
                {
                    objectOnSlot.Quantity += droppedObject.GetComponent<Item>().Quantity;
                    Destroy(droppedObject);
                }
            }
        } else if (droppedObject != null && objectOnSlot == null && AcceptedItems.Contains(droppedObject.GetComponent<Item>().GetItemType()))
        {
            droppedObject.GetComponent<Item>().IsDroppedOnSlot = true;
            droppedObject.GetComponent<Item>().SetParent(transform);
            if(_isEquipSlot)
                droppedObject.GetComponent<Item>().OnEquip();
            droppedObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}
