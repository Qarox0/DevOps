using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Code.Utils;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TranslationUtils: Editor
{
    [MenuItem("Translation Tools/Generate Keys List")]
    public static void GenerateKeys()
    {
        #region GatheringKeys

        List<string> TranslationKeys = new List<string>();
        foreach (var sceneGUID in AssetDatabase.FindAssets("t:Scene", new string[] {"Assets"}))
        {
            var scenePath = AssetDatabase.GUIDToAssetPath(sceneGUID);
            EditorSceneManager.OpenScene(scenePath);
            
            foreach (var translatable in FindObjectsOfType<Translatable>(true))
            {
                var temp = translatable.gameObject.GetComponent<TMP_Text>();
                if (temp != null)
                    TranslationKeys.Add(temp.text);
            }
        }
        EventObject[] events = Resources.LoadAll<EventObject>(GlobalConsts.PathToEvents);
        foreach (var _event in events)
        {
            TranslationKeys.Add(_event._eventName);
            TranslationKeys.Add(_event._eventDescription);
        }
        
        AnswerObject[] answers = Resources.LoadAll<AnswerObject>(GlobalConsts.PathToAnswers);
        foreach (var answer in answers)
        {
            TranslationKeys.Add(answer.AnswerDescription);
        }
        GameObject[] gameObjects = Resources.LoadAll<GameObject>(GlobalConsts.PathToItems);
        foreach (var gameObject in gameObjects)
        {
            var item = gameObject.GetComponent<Item>();
            if(item != null)
            {
                TranslationKeys.Add(item._name);
                TranslationKeys.Add(item._decription);
            }
        }

        foreach (var quality in SettingsPanelManager.LIST_OF_QUALITIES)
        {
            TranslationKeys.Add(quality);
        }
        foreach (var settings in SettingsPanelManager.LIST_OF_FULLSCREEN_OPTIONS)
        {
            TranslationKeys.Add(settings);
        }
        foreach (var lang in SettingsPanelManager.LIST_OF_LANGUAGES_OPTIONS)
        {
            TranslationKeys.Add(lang);
        }

        #endregion

        #region Saving To XML

        int            i           = 0;
        XmlDocument    document    = new XmlDocument();
        XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
        XmlElement     root        = document.DocumentElement;
        document.InsertBefore(declaration, root);
        XmlElement translationElement = document.CreateElement("Translation");
        document.AppendChild(translationElement);
        translationElement.SetAttribute("Lang", "XX_xx");
        foreach (var key in TranslationKeys)
        {
            XmlElement translationPair = document.CreateElement("Pair");
            translationPair.SetAttribute("Key", key);
            translationPair.SetAttribute("Value", "T");
            translationElement.AppendChild(translationPair);
            i++;
        }
        document.Save(GlobalConsts.PathToSaves+"XX_xx.xml");
        Debug.Log($"Generated {i} keys");
        #endregion

    }

    [MenuItem("Translation Tools/Generate Translatable Component")]
    public static void AddTranslateableIfDontExits()
    {
        foreach (var o in FindObjectsOfType(typeof(TMP_Text),true))
        {
            Debug.Log("Found");
            var text = (TMP_Text) o;
            if (text != null && text.gameObject.GetComponent<Untranslatable>() == null && text.gameObject.GetComponent<Translatable>() == null)
            {
                text.gameObject.AddComponent<Translatable>();
            }
        }

    }
}
