using System.Collections;
using System.Collections.Generic;
using Code.Utils;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AlchemyManager : MonoBehaviour
{
    [SerializeField] private List<AlchemyRecipeObject> _alchemyRecipes;
    [SerializeField] private GameObject                _outputSlot;
    [SerializeField] private List<GameObject>          _craftingSlots;
    [SerializeField] private Button                    _buttonCraft;
    [SerializeField] private Button                    _closeButton;
    [SerializeField] private GameObject                _alchemyPanel;
    private void Start()
    {
        _buttonCraft.onClick.AddListener(CraftClick);
        _closeButton.onClick.AddListener(()=>Close());
    }

    private void Close()
    {
        _alchemyPanel.SetActive(false);
        Clear();
    }

    private void Clear()
    {
        var allSlots = _craftingSlots;
        allSlots.Add(_outputSlot);
        foreach (var slot in allSlots)
        {
            if (slot.transform.childCount > 0)
            {
                var item = slot.transform.GetChild(0).GetComponent<Item>();
                for (int i = 0; i < item.Quantity; i++)
                {
                    Inventory.GetInventoryInstance().AddItemToInventory(Resources.Load<GameObject>(GlobalConsts.PathToItems+item.PrefabName));
                }
                Destroy(item.gameObject);
            }
        }
    }

    private void CraftClick()
    {
        var recipe = FindRecipe();
        if (recipe != null)
        {
            var output = Instantiate(recipe.OutputPrefab, _outputSlot.transform);
            output.GetComponent<Item>().Quantity = recipe.OutputAmount;
        } 
        foreach (var slot in _craftingSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }
    }
    
    private AlchemyRecipeObject FindRecipe()
    {
        var reducedRecipeList = _alchemyRecipes;
        int i                 = 1;
        foreach (var slot in _craftingSlots)
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
    private List<AlchemyRecipeObject> ReduceList(List<AlchemyRecipeObject> listToReduce, Item slotItem, int slotNumber)
    {
        List<AlchemyRecipeObject> reducedRecpieList = new List<AlchemyRecipeObject>();
        foreach (var recipe in listToReduce)
        {
            switch (slotNumber)
            {
                case 1:
                    if (slotItem == null &&
                        recipe.ItemPrefab1 == null)
                    {
                        reducedRecpieList.Add(recipe);
                    }else if(slotItem != null &&
                             recipe.ItemPrefab1 != null &&
                             recipe.Amount1 == slotItem.Quantity &&
                             recipe.ItemPrefab1.GetComponent<Item>().GetName() == slotItem.GetName())
                    {
                        reducedRecpieList.Add(recipe);
                    }
                    break;
                case 2:
                    if (slotItem            == null &&
                        recipe.ItemPrefab2 == null)
                    {
                        reducedRecpieList.Add(recipe);
                    }else if(slotItem                                           != null              &&
                             recipe.ItemPrefab2                                != null              &&
                             recipe.Amount2                                    == slotItem.Quantity &&
                             recipe.ItemPrefab2.GetComponent<Item>().GetName() == slotItem.GetName())
                    {
                        reducedRecpieList.Add(recipe);
                    }
                    break;
                case 3:
                    if (slotItem            == null &&
                        recipe.ItemPrefab3 == null)
                    {
                        reducedRecpieList.Add(recipe);
                    }else if(slotItem                                           != null              &&
                             recipe.ItemPrefab3                                != null              &&
                             recipe.Amount3                                    == slotItem.Quantity &&
                             recipe.ItemPrefab3.GetComponent<Item>().GetName() == slotItem.GetName())
                    {
                        reducedRecpieList.Add(recipe);
                    }
                    break;
            }
        }

        return reducedRecpieList;

    }
}
