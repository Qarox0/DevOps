using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEdible : MonoBehaviour, IEdible
{
    
    public void Eat(Item item)
    {
        Debug.Log($"yummy: {item}");
    }
}
