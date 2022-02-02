using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugResult : MonoBehaviour, IEventResult
{
    public void DoResult()
    {
        Debug.Log("This is debug result");
        EventManager.GetInstance().CloseEvent();
    }
}
