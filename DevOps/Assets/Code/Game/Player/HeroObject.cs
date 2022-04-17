using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Hero", menuName = "Scriptable/New Hero")]
public class HeroObject : ScriptableObject
{
   public string           Name;
   public string           Description;
   public Sprite           HeroImage;
   public List<StatsBonus> StatsBonuses;
   public string           HeroDefName;

}
