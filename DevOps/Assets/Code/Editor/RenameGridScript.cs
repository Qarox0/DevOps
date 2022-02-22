using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RenameGridScript
{
    [MenuItem("LD Tools/Rename Grid")]
    public static void Rename()
    {
        var obj = Selection.activeObject as GameObject;
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            obj.transform.GetChild(i).name = $"Hex{i}";
        }
    }
    [MenuItem("LD Tools/Attach Save")]
    public static void AttachSave()
    {
        var obj = Selection.activeObject as GameObject;
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            if (obj.transform.GetChild(i).GetComponent<SaveableEntity>() == null)
            {
                var entity = obj.transform.GetChild(i).gameObject.AddComponent<SaveableEntity>();
                entity.SetID(Guid.NewGuid().ToString());
            }
        }
    }
    [MenuItem("LD Tools/ID/Generate ID For Empty")]
    public static void GenerateForEmpty()
    {
        var obj = Selection.activeObject as GameObject;
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            var entity = obj.transform.GetChild(i).GetComponent<SaveableEntity>();
            if (entity != null && entity.ID == String.Empty)
            {
                entity.SetID(Guid.NewGuid().ToString());
            }
        }
    }
    [MenuItem("LD Tools/ID/Generate ID For All")]
    public static void GenerateForAll()
    {
        var obj = Selection.activeObject as GameObject;
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            var entity = obj.transform.GetChild(i).GetComponent<SaveableEntity>();
            if (entity != null)
            {
                entity.SetID(Guid.NewGuid().ToString());
            }
        }
    }
}
