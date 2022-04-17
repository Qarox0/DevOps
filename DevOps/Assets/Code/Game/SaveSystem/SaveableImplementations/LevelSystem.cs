using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour, ISaveable
{
    [SerializeField] private int level = 1;
    [SerializeField] private int xp    = 100;
    public object CaptureState()
    {
        return new SaveData
        {
            level = level,
            xp    = xp
        };
    }

    public void   RestoreState(object state)
    {
        var data = (SaveData) state;

        level = data.level;
        xp    = data.xp;
    }

    [Serializable]
    private struct SaveData
    {
        public int level;
        public int xp;
    }
}
