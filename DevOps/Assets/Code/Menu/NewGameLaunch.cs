using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameLaunch : MonoBehaviour
{

    private float time = 0;

    private bool launched = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 10 && !launched)
        {
            if (PlayerPrefs.HasKey("NewGame") && PlayerPrefs.GetInt("NewGame") == 1)
            {
                EventManager.GetInstance().LaunchEvent("StartEvent");
            }

            launched = true;
        }
    }
}
