using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable/New Event", fileName = "Event")]
public class EventObject : ScriptableObject
{
    public string       _eventName        = "";
    public string       _eventDescription = "";
    public List<String> _answerList;
    public Sprite       _eventSprite;


}
