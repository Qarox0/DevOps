using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Code.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageManager : MonoBehaviour
{
    private static LanguageManager _instance;

    private Dictionary<string, string> translatedDictionary;

    private string _currentLang = "EN_us";

    private static readonly string[] LIST_OF_LANGS = new[]
    {
        "EN_us", "PL_pl", "DE_de", "ZH_zh", "PT_pt", "ES_es"

    };

    public static LanguageManager GetInstance()
    {
        if (_instance == null) _instance = FindObjectOfType<LanguageManager>();
        return _instance;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
        LoadTranslation();
        SceneManager.sceneLoaded += OnSceneLoaded;
        foreach (var translatable in FindObjectsOfType<Translatable>())
        {
            translatable.GetComponent<TMP_Text>().text = GetTranslation(translatable.GetComponent<TMP_Text>().text);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var translatable in FindObjectsOfType<Translatable>())
        {
            translatable.GetComponent<TMP_Text>().text = GetTranslation(translatable.GetComponent<TMP_Text>().text);
        }
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetLang(int langNumber)
    {
        _currentLang = LIST_OF_LANGS[Mathf.Clamp(langNumber, 0, LIST_OF_LANGS.Length - 1)];
    }
    public string GetTranslation(string key)
    {
        string translation = "";
        if (translatedDictionary.ContainsKey(key))
            translation = translatedDictionary[key];
        return translation;
    }

    private void LoadTranslation()
    {
        translatedDictionary = new Dictionary<string, string>();
        #if UNITY_EDITOR
        TextAsset   textAsset = (TextAsset) Resources.Load($"{GlobalConsts.PathToTranslations}{GlobalConsts.DebugLanguage}");  
        #else
        TextAsset   textAsset = (TextAsset) Resources.Load($"{GlobalConsts.PathToTranslations}{_currentLang}");
        #endif

        XmlDocument xmldoc    = new XmlDocument ();
        xmldoc.LoadXml ( textAsset.text );
        XmlNodeList dataNodes = xmldoc.SelectNodes("//Translation");

        foreach (XmlNode node in dataNodes)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Attributes["Key"] != null && childNode.Attributes["Value"] != null)
                {
                    translatedDictionary.Add(childNode.Attributes["Key"].Value, childNode.Attributes["Value"].Value);
                }
            }
        }
    }
    
}
