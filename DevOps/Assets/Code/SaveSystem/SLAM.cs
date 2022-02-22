using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Code.Utils;
using UnityEngine;

public class SLAM : MonoBehaviour
{
    public         Action OnSaveLoaded;
    private static SLAM   _instance;

    public static SLAM GetInstance()
    {
        if (_instance == null) _instance = FindObjectOfType<SLAM>();
        return _instance;
    }

    
    public void Save(string savePath)
    {
        var state = LoadFile(savePath);
        CaptureState(state);
        SaveFile(state, savePath);
    }

    public void Load(string savePath)
    {
        var state = LoadFile(savePath);
        RestoreState(state);
        OnSaveLoaded.Invoke();
    }
    
    private void SaveFile(object state,string savePath)
    {
        if(!Directory.Exists(GlobalConsts.PathToSaves))
        {
            Directory.CreateDirectory(GlobalConsts.PathToSaves);
        }
        using (var stream = File.Open(savePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream,state);
        }
    }

    private Dictionary<string, object> LoadFile(string savePath)
    {
        
        if (!File.Exists(savePath) || !Directory.Exists(GlobalConsts.PathToSaves))
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(savePath,FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>) formatter.Deserialize(stream);
        }
    }

    private void CaptureState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.ID] = saveable.CaptureState();
        }
    }

    private void RestoreState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            if (state.TryGetValue(saveable.ID, out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }
}
