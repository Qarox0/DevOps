using System;
using System.Linq;
using Code.Utils;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestObject))]
class GoalInspector : Editor
{
    private Type[] _implementations; //tablica dostepnych implementacji
    private int    _implementationTypeIndex; //wybrana implementacja

    public override void OnInspectorGUI()
    {
        //Znajdź obiekt
        QuestObject questObject = target as QuestObject;
        
        if (questObject == null)
        {
            return;
        }
        
        //jeśli implementacje są puste lub wciśnięto przycisk
        if (_implementations == null || GUILayout.Button("Refresh implementations"))
        {
            //znajdź implementacje z podklas unityengine.object
            _implementations = AssembliesUtils.GetImplementations<Goal>().Where(impl=>!impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
        }
        
        //Napisz ile znaleziono
        EditorGUILayout.LabelField($"Found {_implementations.Count()} implementations");            
        
        //Daj listę do wyboru
        _implementationTypeIndex = EditorGUILayout.Popup(new GUIContent("Implementation"),
                                                         _implementationTypeIndex, _implementations.Select(impl => impl.FullName).ToArray());

        //po wcisnięciu przycisku
        if (GUILayout.Button("Create instance"))
        {
            //dodaj wybraną implementacje
            questObject.goal.Add((Goal) Activator.CreateInstance(_implementations[_implementationTypeIndex]));
        }
        
        //użyj dalej bazowego edytora
        base.OnInspectorGUI();
    }
    
}