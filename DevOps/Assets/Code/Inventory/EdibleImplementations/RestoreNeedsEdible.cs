using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreNeedsEdible : MonoBehaviour, IEdible
{
    private string _hungerKey = "Hunger";
    private string _thirstKey = "Thirst";
    public void Eat(string _params,Item item)
    {
        int                            hunger = 0;
        int                            thirst = 0;
            Dictionary<string, string> dict   = _params.HandleParams();
        if (dict.ContainsKey(_hungerKey))
        {
            hunger = int.Parse(dict[_hungerKey]);
        }
        if (dict.ContainsKey(_thirstKey))
        {
            thirst = int.Parse(dict[_thirstKey]);
        }
        NeedsHandler.GetInstance().RestoreNeeds(hunger,thirst);
        Inventory.GetInventoryInstance().SubstractFromInventory(item, 1);
        
    }
}
