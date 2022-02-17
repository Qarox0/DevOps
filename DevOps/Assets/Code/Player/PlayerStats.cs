using Unity.Mathematics;
using UnityEngine;

public class PlayerStats
{
    public float Health       { get; set; }
    public float ActualHealth { get; set; }
    public float FatalRisk    { get; set; }
    public float Luck         { get; set; }

    private float _torsoDamage;

    private float _hunger;

    private float _thirsty;
    public float TorsoDamage
    {
        get => _torsoDamage;
        set => _torsoDamage = Mathf.Clamp(value, 0, 100);
    } 
    private float _headDamage;
    public float HeadDamage
    {
        get => _headDamage;
        set => _headDamage = Mathf.Clamp(value, 0, 100);
    } 
    private float _leftLegDamage;
    public float LeftLegDamage
    {
        get => _leftLegDamage;
        set => _leftLegDamage = Mathf.Clamp(value, 0, 100);
    } 
    private float _rightLegDamage;
    public float RightLegDamage
    {
        get => _rightLegDamage;
        set => _rightLegDamage = Mathf.Clamp(value, 0, 100);
    } 
    private float _sanity;
    public float Sanity
    {
        get => _sanity;
        set => _sanity = Mathf.Clamp(value, 0, 100);
    }
    public float Thirsty
    {
        get => _thirsty;
        set => _thirsty = Mathf.Clamp(value, 0, 100);
    }

    public float Hunger
    {
        get => _hunger;
        set => _hunger = Mathf.Clamp(value, 0, 100);
    }

    
}