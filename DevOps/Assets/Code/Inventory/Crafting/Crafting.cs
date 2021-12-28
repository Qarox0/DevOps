using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    [SerializeField] private List<GameObject>   _craftingSlots;
    [SerializeField] private GameObject         _outputSlot;
    [SerializeField] private Button             _buttonCraft;
    [SerializeField] private List<RecipeObject> _listOfRecipes;

    private void Start()
    {
        _buttonCraft.onClick.AddListener(CraftClick);
    }

    private void CraftClick()
    {
        var recipe = FindRecipe();
        if (recipe != null)
        {
            var output = Instantiate(recipe._outputPrefab, _outputSlot.transform);
            output.GetComponent<Item>().Quantity = recipe._outputAmount;
        } 
        foreach (var slot in _craftingSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }
    }

    private RecipeObject FindRecipe()
    {
        var reducedRecipeList = _listOfRecipes;
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

    private List<RecipeObject> ReduceList(List<RecipeObject> listToReduce, Item slotItem, int slotNumber)
    {
        List<RecipeObject> reducedRecpieList = new List<RecipeObject>();
        foreach (var recipe in listToReduce)
        {
            switch (slotNumber)
            {
                case 1:
                    if (slotItem == null &&
                        recipe._itemPrefab1 == null)
                    {
                        reducedRecpieList.Add(recipe);
                    }else if(slotItem != null &&
                             recipe._itemPrefab1 != null &&
                             recipe._amount1 == slotItem.Quantity &&
                             recipe._itemPrefab1.GetComponent<Item>().GetName() == slotItem.GetName())
                    {
                        reducedRecpieList.Add(recipe);
                    }
                    break;
                case 2:
                    if (slotItem            == null &&
                        recipe._itemPrefab2 == null)
                    {
                        reducedRecpieList.Add(recipe);
                    }else if(slotItem                                           != null              &&
                             recipe._itemPrefab2                                != null              &&
                             recipe._amount2                                    == slotItem.Quantity &&
                             recipe._itemPrefab2.GetComponent<Item>().GetName() == slotItem.GetName())
                    {
                        reducedRecpieList.Add(recipe);
                    }
                    break;
                case 3:
                    if (slotItem            == null &&
                        recipe._itemPrefab3 == null)
                    {
                        reducedRecpieList.Add(recipe);
                    }else if(slotItem                                           != null              &&
                             recipe._itemPrefab3                                != null              &&
                             recipe._amount3                                    == slotItem.Quantity &&
                             recipe._itemPrefab3.GetComponent<Item>().GetName() == slotItem.GetName())
                    {
                        reducedRecpieList.Add(recipe);
                    }
                    break;
                case 4:
                    if (slotItem            == null &&
                        recipe._itemPrefab4 == null)
                    {
                        reducedRecpieList.Add(recipe);
                    }else if(slotItem                                           != null              &&
                             recipe._itemPrefab4                                != null              &&
                             recipe._amount4                                    == slotItem.Quantity &&
                             recipe._itemPrefab4.GetComponent<Item>().GetName() == slotItem.GetName())
                    {
                        reducedRecpieList.Add(recipe);
                    }
                    break;
                case 5:
                    if (slotItem            == null &&
                        recipe._itemPrefab5 == null)
                    {
                        reducedRecpieList.Add(recipe);
                    }else if(slotItem                                           != null              &&
                             recipe._itemPrefab5                                != null              &&
                             recipe._amount5                                    == slotItem.Quantity &&
                             recipe._itemPrefab5.GetComponent<Item>().GetName() == slotItem.GetName())
                    {
                        reducedRecpieList.Add(recipe);
                    }
                    break;
            }
        }

        return reducedRecpieList;

    }


}
