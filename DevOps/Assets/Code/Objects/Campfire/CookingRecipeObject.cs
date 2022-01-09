using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CookingRecipe", menuName = "Scriptable/New Cooking Recipe")]
public class CookingRecipeObject : ScriptableObject
{
    public RequiredItem ItemNeeded1;
    public RequiredItem ItemNeeded2;
    public RequiredItem ItemNeeded3;
    public RequiredItem RawOutput;
    public RequiredItem Output;
    public RequiredItem FailureOutput;
    public int          MinimalTime;
    public int          MaximalTime;
    
}
