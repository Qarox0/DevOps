using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEdible : MonoBehaviour, IEdible
{
    
    public void Eat(string _params, Item item)
    {
        Debug.Log($"yummy: {_params}");
    }
}
