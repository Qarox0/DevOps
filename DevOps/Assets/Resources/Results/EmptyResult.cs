using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyResult : MonoBehaviour, IEventResult
{
    public void DoResult(string _params)
    {
        EventManager.GetInstance().CloseEvent();
    }
}
