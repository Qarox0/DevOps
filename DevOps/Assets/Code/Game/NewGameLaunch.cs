using System.Collections;
using System.Collections.Generic;
using Code.Utils;
using UnityEngine;

public class NewGameLaunch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("NewGame") && PlayerPrefs.GetInt("NewGame") == 1)
            EventManager.GetInstance().LaunchEvent("StartEvent");
    }

    // Update is called once per frame
    void Update()
    {
            
    }
}
