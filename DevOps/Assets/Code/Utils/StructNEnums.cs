using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[Serializable]
public struct CatchEnum
{
    public string EventName;
    public float  EventChance;
}

[Serializable]
public struct RequiredItem
{
    public String ItemNeeded;
    public int        Amount;

    public void SetAmount(int value)
    {
        Amount = value;
    }

    public void SetItem(string gameObject)
    {
        ItemNeeded = gameObject;
    }

    public RequiredItem(int amount = 0, string item = "")
    {
        Amount     = amount;
        ItemNeeded = item;
        return;
    }
}

public enum ConfirmationContextEnum
{
    OVERWRITE,EXIT,NEW_GAME
}

public enum QualitySettingsNames
{
    VERY_LOW,LOW,MEDIUM,HIGH,VERY_HIGH, ULTRA
}

public enum Biomes
{
    GRASSLAND, BEATCH
}
public enum Flora
{
    NULL, FOREST, MEADOW
}
[Serializable]
public struct GenerateLevelDiversity
{
    public Color            MarkColor;
    [Range(0,1)]
    public float            MinValue;
    [Range(0, 1)]
    public float            MaxValue;
    public float            ChanceOfObject;
    public List<GameObject> FloraList;
}

[Serializable]
public struct Weather
{
    public WeatherType Type;
    [Range(0,1)]
    public float         MaxOccurenceChance;
    [Range(0, 1)]
    public float         MinOccurenceChance;
    public             int MinimalDuration;
}

public enum WeatherType
{
    SUN,RAIN
}