using System;
using System.Collections.Generic;
using OK.StatsSystem;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class PlayerStats
{
    

    public SAttribute Strength;
    public SAttribute Toughness;
    public SAttribute Reflex;
    public SAttribute Intelligence;
    public SAttribute Karma;
    public SAttribute Exactitude;
    public SAttribute MaxHunger;
    public SAttribute MaxThirst;
    public SAttribute MaxWeight;
    public SAttribute MaxHealth;
    private float _weight;
    private float _hunger;
    private float _thirst;
    private float _health;
    public float Hunger
    {
        get { return _hunger;}
        set { _hunger = Mathf.Clamp(value, 0, MaxHunger.Value); }
    }
    public float Thirst
    {
        get { return _thirst;}
        set { _thirst = Mathf.Clamp(value, 0, MaxThirst.Value); }
    }
    public float Weight
    {
        get { return _weight;}
        set { _weight = Mathf.Clamp(value, 0, MaxWeight.Value); }
    }
    public float Health
    {
        get { return _health;}
        set { _health = Mathf.Clamp(value, 0, MaxHealth.Value); }
    }

    
}