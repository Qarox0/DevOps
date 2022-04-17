using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable/New Answer", fileName = "EventAnswer")]
public class AnswerObject : ScriptableObject
{
    public string ResultNames        = "";
    public string AnswerDescription = "";
    public string Params            = "";
}
