using System;
using UnityEngine;
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