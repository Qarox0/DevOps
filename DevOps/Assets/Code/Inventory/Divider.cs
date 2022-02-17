using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Divider : MonoBehaviour
{
    [SerializeField] private GameObject _dividerPanel;
    [SerializeField] private Slider     _slider;
    [SerializeField] private TMP_Text   _dividerText;

    private static Divider _instance;

    public static Divider GetInstance()
    {
        if (_instance == null) _instance = FindObjectOfType<Divider>();
        return _instance;
    }

    public void init(GameObject draggedItem)
    {
    }
    
    
}
