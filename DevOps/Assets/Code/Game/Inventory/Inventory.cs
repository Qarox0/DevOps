using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Code.Utils;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, ISaveable
{
    [Header("Inventory Properties")]
    [Tooltip("How many slots have to be generated")]
    [SerializeField] private int              _slotNumber = 9;          //ile slotów ma być generowane na start
    [Tooltip("Parent for slots")]
    [SerializeField] private Transform        _parentInventoryTransform;//Ojciec slotów, czyli pod co są podpinane
    [Tooltip("Prefab of slot which is instantiated")]
    [SerializeField] private GameObject       _slotPrefab;              //Prefab Slotów
    [Space]
    [Header("DEBUG")]
    [Tooltip("DEBUG: list of slots")]
    [SerializeField] private List<GameObject> _slotsList;               //Lista Slotów
    [SerializeField] private Slider _weightSlider;

    private FunctionTimer _weightRefreshTimer;
    // Start is called before the first frame update
    private static Inventory _instance;                                 //instancja ekwipunku
    public static Inventory GetInventoryInstance()                      //Singleton Ekwipunku
    {
        if (_instance == null) _instance = FindObjectOfType<Inventory>();   //Znajdź Instancje eq w hierarchii
        return _instance;                                                   //zwróć instancje
    }
    void Start()
    {
        for (int i = 0; i < _slotNumber; i++)       //generacja slotów
        {
            _slotsList.Add(Instantiate(_slotPrefab, _parentInventoryTransform));
        }
        _weightRefreshTimer = FunctionTimer.Create(()=>CalculateWeight(),1f,false);
    }

    private void OnDestroy()
    {
        _weightRefreshTimer.Stop();
    }


    #region public
    public void AddItemToInventory(GameObject itemPrefab) //dodaje item do ekwipunku
    {
        var slotWithSameItem = SearchInSlotsWithNoFullQuantity(itemPrefab.GetComponent<Item>());
        if (slotWithSameItem != null)
        {
            slotWithSameItem.GetComponentInChildren<Item>().Quantity++;
        }
        else
        {
            var emptySlot = FindEmptySlot();
            if (emptySlot != null)
            {
                var  itemObject = Instantiate(itemPrefab, emptySlot.transform);
                itemObject.GetComponent<Item>().Quantity++;
                
            }
            else
            {
                //TODO Implementacja pełnego ekwipunku
            }
        }

        var player = Player.GetInstance();
        player.Tell($"<color=\"red\">+1 {itemPrefab.GetComponent<Item>()._name}</color>");
        player.stats.Weight = CalculateWeight();
        //TODO Recalculate max weight on time
    }

    public bool IsHaving(Item item)     //czy posiada podany item
    {
        var itemSlot = this.SearchInSlots(item);
        return itemSlot != null;
    }
    public bool SubstractFromInventory(Item item, int substractCount) //odejmuje podany item z ekwipunku
    {
        bool isDone      = false;
        var  actualCount = substractCount;
        if (GetSumOfItem(item) >= substractCount)
        {
            foreach (var slot in _slotsList)
            {
                if (slot.transform.childCount == 1)
                {
                    var child      = slot.transform.GetChild(0);
                    var itemInSlot = child.GetComponent<Item>();
                    if (itemInSlot.GetName() == item.GetName())
                    {
                        int toSubstract = Mathf.Clamp(actualCount, 0, item.GetMaxStackQuantity());
                        this.SubstractItem(slot, toSubstract);
                        actualCount -= toSubstract;
                        if (actualCount <= 0)
                        {
                            break;
                        }


                    }
                }
            }
        }
        Player.GetInstance().stats.Weight = CalculateWeight();
        return isDone;
    }

    public bool CheckForBuildingRecipe(BuildingRecipeObject recipe)
    {
        int i = 0;
        foreach (var item in recipe.ItemsNeeded)
        {
            if (GetSumOfItem(Resources.Load<GameObject>(GlobalConsts.PathToItems +item.ItemNeeded).GetComponent<Item>()) >= item.Amount)
            {
                i++;
            }
            
        }

        return i == recipe.ItemsNeeded.Count;
    }
    #endregion

    #region private
    private GameObject SearchInSlots(Item item)              //wyszukuje item podany we wszystkich slotach
    {
        GameObject slotWithItem = null;
        foreach (var slot in _slotsList)
        {
            if (slot.transform.childCount == 1)
            {
                var child = slot.transform.GetChild(0);
                if (child.GetComponent<Item>().GetName() == item.GetName())
                {
                    slotWithItem = slot;
                    break;
                }
            }
        }

        return slotWithItem;
    }
    private int GetSumOfItem(Item item)                      //Zbiera sume podanego itemu w eq
    {
        int sum = 0;
        foreach (var slot in _slotsList)
        {
            if (slot.transform.childCount == 1)
            {
                var child      = slot.transform.GetChild(0);
                var itemInSlot = child.GetComponent<Item>();
                if (itemInSlot.GetName() == item.GetName())
                {
                    sum += itemInSlot.Quantity;
                }
            }
        }
        return sum;
    }
    private GameObject SearchInSlotsWithNoFullQuantity(Item item)    //Wyszukuje item podany we wszystkich slotach i szuka nie pełnego
    {
        GameObject slotWithItem = null;
        foreach (var slot in _slotsList)
        {
            if (slot.transform.childCount == 1)
            {
                var child = slot.transform.GetChild(0);
                var itemInSlot  = child.GetComponent<Item>();
                if (itemInSlot.GetName() == item.GetName() && itemInSlot.Quantity < itemInSlot.GetMaxStackQuantity())
                {
                    slotWithItem = slot;
                    break;
                }
            }
        }

        return slotWithItem;
    }
    private float CalculateWeight()          //oblicza wagę całego ekwipunku
    {
        float weight = 0;
        foreach (var slot in _slotsList)
        {
            if (slot.transform.childCount == 1)
            {
                var child = slot.transform.GetChild(0);
                weight += child.GetComponent<Item>().GetWeight() * child.GetComponent<Item>().Quantity;
            }
        }

        _weightSlider.value = Mathf.Clamp01(weight / Player.GetInstance().stats.MaxWeight.Value);
        return weight;
    }
    private GameObject FindEmptySlot()      //Znajduje pusty slot
    {
        GameObject emptySlot = null;
        foreach (var slot in _slotsList)
        {
            if (slot.transform.childCount == 0)
            {
                emptySlot = slot;
                break;
            }
        }
        return emptySlot;
    }
    private void SubstractItem(GameObject slotWithItem, int quantity) //handle od odejmowania itemów
    {
        var item = slotWithItem.GetComponentInChildren<Item>();
        item.Quantity -= quantity;
        if (item.Quantity == 0)
        {
            Destroy(slotWithItem.transform.GetChild(0).gameObject);
        }
        CalculateWeight();
    }
    
    
    #endregion

    public object CaptureState()
    {
        Dictionary<int,RequiredItem> itemsInEq = new Dictionary<int, RequiredItem>();
        int                          i         = 0;
        foreach (var slot in _slotsList)
        {
            if (slot.transform.childCount > 0)
            {
                var item = slot.GetComponentInChildren<Item>();
                if (item != null)
                {
                    itemsInEq.Add(i,
                                  new RequiredItem(item.Quantity,
                                                   item.PrefabName));
                }
            }
            else
            {
                itemsInEq.Add(i, new RequiredItem());

            }

            i++;
        }

        foreach (var test in itemsInEq)
        {
        }
        return new InventorySaveData
        {
            items = itemsInEq
        };
    }

    public void RestoreState(object state)
    {
        foreach (var slot in _slotsList)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }
        var data = (InventorySaveData) state;
        int i    = 0;
        foreach (var item in data.items)
        {
            if (!item.Value.Equals(default(RequiredItem)) &&!item.Value.ItemNeeded.Equals(String.Empty))
            {
                    var inst = Instantiate(Resources.Load<GameObject>(GlobalConsts.PathToItems + item.Value.ItemNeeded),
                                           _slotsList[i].transform, false);
                    inst.GetComponent<Item>().Quantity = item.Value.Amount;

            }

            i++;
        }
    }
    [Serializable]
    private struct InventorySaveData
    {
        public Dictionary<int,RequiredItem> items;
    }
}
