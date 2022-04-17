using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Quest Object", menuName = "Scriptable/Quest/New Quest Object")]
public class QuestObject : ScriptableObject
{
    [SerializeReference] public List<Goal> Goals;
}
