using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainResult : MonoBehaviour, IEventResult
{
    public void DoResult(string _params)
    {
        var dictionary = _params.HandleParams();
        if (dictionary.ContainsKey("_eventName"))
        {
            EventManager.GetInstance().CloseEvent();
            EventManager.GetInstance().LaunchEvent(dictionary["_eventName"]);
        }
        else
        {
            Debug.LogError($"{this.name} don't have proper params");
        }
    }
}
