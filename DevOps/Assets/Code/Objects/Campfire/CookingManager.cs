using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingManager : MonoBehaviour
{
    [SerializeField] private GameObject                _cookingPanel;
    [SerializeField] private Button                    _cookButton;
    [SerializeField] private List<CookingRecipeObject> _recipes;
    [SerializeField] private List<GameObject>          _cookingSlots;
    [SerializeField] private GameObject                _outputSlot;
    [SerializeField] private GameObject                _fuelSlot;
    private                  CampfireHex               _actualCampfire;
    private static           CookingManager            _instance;
    public                   bool                      IsOpen { get; private set; }

    private void Start()
    {
        _cookButton.onClick.AddListener(Cook);
        FindObjectOfType<Player>().onPlayerMove += PlayerMoved;
    }

    private void PlayerMoved()
    {
        if(IsOpen) Close();
        _cookingPanel.SetActive(false);
    }

    private void ClearCooking()
    {
        foreach (var slot in _cookingSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }
        if(_fuelSlot.transform.childCount > 0) Destroy(_fuelSlot.transform.GetChild(0).gameObject);
        if(_outputSlot.transform.childCount > 0) Destroy(_outputSlot.transform.GetChild(0).gameObject);
        SetCookingVisual(false);
        _actualCampfire                         = null;
    }

    public static CookingManager GetInstance()
    {
        if (_instance == null) _instance = FindObjectOfType<CookingManager>();
        return _instance;
    }

    public void SetCookingVisual(bool isCooking)
    {
        if (isCooking)
        {
            _outputSlot.GetComponent<Image>().color = Color.red;
        }
        else
        {
            _outputSlot.GetComponent<Image>().color = Color.white;
        }
    }

    public void Init(CampfireHex campfire)
    {
        _actualCampfire = campfire;
        IsOpen         = true;
        _cookingPanel.SetActive(true);
        if (_actualCampfire.Recipe != null)
        {
            var item = Instantiate(_actualCampfire.Output.ItemNeeded, _outputSlot.transform);
            item.GetComponent<Item>().Quantity = _actualCampfire.Output.Amount;
            if (_actualCampfire.GetFuelPrefab() != null && _actualCampfire.GetFuelPrefab().GetComponent<Item>().Quantity > 1)
            {
                var fuel = Instantiate(_actualCampfire.GetFuelPrefab(), _fuelSlot.transform);
                fuel.GetComponent<Item>().Quantity = _actualCampfire.FuelAmount-1;
                
            }
        }
        SetCookingVisual(_actualCampfire.IsCooking);
    }

    public void Close()
    {
        _actualCampfire.OnClose(this);
        ClearCooking();
        IsOpen = false;
    }

    public bool IsOutputClean()
    {
        return !(_outputSlot.transform.childCount > 0);
    }

    public bool IsFuelClean()
    {
        return !(_fuelSlot.transform.childCount > 0);

    }

    public void Cook()
    {
        var recipe = FindRecipe();
        if (recipe != null && _fuelSlot.transform.childCount > 0)
        {
            _actualCampfire.FirstSlotItem  = recipe.ItemNeeded1;
            _actualCampfire.SecondSlotItem = recipe.ItemNeeded2;
            _actualCampfire.ThirdSlotItem  = recipe.ItemNeeded3;
            _actualCampfire.FuelAmount     = _fuelSlot.GetComponentInChildren<Item>().Quantity;
            _actualCampfire.Recipe         = recipe;
            _actualCampfire.StartCooking(_fuelSlot.GetComponentInChildren<Item>().gameObject);
            SetCookingVisual(true);
            var item = Instantiate(recipe.RawOutput.ItemNeeded, _outputSlot.transform);
            item.GetComponent<Item>().Quantity = recipe.RawOutput.Amount;
        }
        foreach (var slot in _cookingSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
            
        }
        if (_fuelSlot.transform.childCount > 0)
        {
            _fuelSlot.GetComponentInChildren<Item>().Quantity--;
            _actualCampfire.AddBurningTime(_fuelSlot.GetComponentInChildren<Item>()._fuelValue);
            if (_fuelSlot.GetComponentInChildren<Item>().Quantity == 0)
            {
                Destroy(_fuelSlot.transform.GetChild(0).gameObject);
            }
        }
        
    }
    
    private CookingRecipeObject FindRecipe()
    {
        var reducedRecipeList = _recipes;
        int i                 = 1;
        foreach (var slot in _cookingSlots)
        {
            if(slot.GetComponentInChildren<Item>() != null)
            {
                reducedRecipeList = ReduceList(reducedRecipeList, slot.GetComponentInChildren<Item>(), i);

            }
            else
            {
                reducedRecipeList = ReduceList(reducedRecipeList, null, i);
            }
            i++;
        }
        if (reducedRecipeList.Count == 1)
        {
            return reducedRecipeList[0];
        }
        else
        {
            return null;
        }
    }
    
    private List<CookingRecipeObject> ReduceList(List<CookingRecipeObject> listToReduce, Item slotItem, int slotNumber)
    {
        List<CookingRecipeObject> reducedRecpieList = new List<CookingRecipeObject>();
        foreach (var recipe in listToReduce)
        {
            switch (slotNumber)
            {
                case 1:
                    if (slotItem == null &&
                        recipe.ItemNeeded1.ItemNeeded == null)
                    {
                        reducedRecpieList.Add(recipe);
                    }else if(slotItem                                                     != null              &&
                             recipe.ItemNeeded1.ItemNeeded                                != null              &&
                             recipe.ItemNeeded1.Amount                                    == slotItem.Quantity &&
                             recipe.ItemNeeded1.ItemNeeded.GetComponent<Item>().GetName() == slotItem.GetName())
                    {
                        reducedRecpieList.Add(recipe);
                    }
                    break;
                case 2:
                    if (slotItem                      == null &&
                        recipe.ItemNeeded2.ItemNeeded == null)
                    {
                        reducedRecpieList.Add(recipe);
                    }else if(slotItem                                                     != null              &&
                             recipe.ItemNeeded2.ItemNeeded                                != null              &&
                             recipe.ItemNeeded2.Amount                                    == slotItem.Quantity &&
                             recipe.ItemNeeded2.ItemNeeded.GetComponent<Item>().GetName() == slotItem.GetName())
                    {
                        reducedRecpieList.Add(recipe);
                    }
                    break;
                case 3:
                    if (slotItem                      == null &&
                        recipe.ItemNeeded3.ItemNeeded == null)
                    {
                        reducedRecpieList.Add(recipe);
                    }else if(slotItem                                                     != null              &&
                             recipe.ItemNeeded3.ItemNeeded                                != null              &&
                             recipe.ItemNeeded3.Amount                                    == slotItem.Quantity &&
                             recipe.ItemNeeded3.ItemNeeded.GetComponent<Item>().GetName() == slotItem.GetName())
                    {
                        reducedRecpieList.Add(recipe);
                    }
                    break;
            }
        }

        return reducedRecpieList;

    }
}
