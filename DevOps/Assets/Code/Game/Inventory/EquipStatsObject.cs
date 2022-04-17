using System.Collections;
using System.Collections.Generic;
using OK.StatsSystem;
using UnityEngine;
[CreateAssetMenu(fileName = "New EquipStat", menuName = "Scriptable/New EquipObject")]
public class EquipStatsObject : ScriptableObject
{
    public List<AModifier> Strength;
    public List<AModifier> Toughness;
    public List<AModifier> Reflex;
    public List<AModifier> Intelligence;
    public List<AModifier> Karma;
    public List<AModifier> Exactitude;
    public List<AModifier> MaxHunger;
    public List<AModifier> MaxThirst;
    public List<AModifier> MaxWeight;
    public List<AModifier> MaxHealth;
}
