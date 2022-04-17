using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New Archeology Data", menuName = "Scriptable/New ArcheologyData")]
public class ArcheologyDataObject : ScriptableObject
{
    public List<Image> ImagesToGenerateList;
    public List<GameObject>  Rewards;
}
