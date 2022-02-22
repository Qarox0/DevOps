using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Code.Utils;
using ICSharpCode.NRefactory.Ast;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TranslationUtils: Editor
{
    [MenuItem("Translation Tools/Generate Keys List")]
    public static void GenerateKeys()
    {
        #region GatheringKeys

        List<string> TranslationKeys = new List<string>();
        foreach (var translatable in FindObjectsOfType<Translatable>())
        {
            var temp = translatable.gameObject.GetComponent<TMP_Text>();
            if(temp != null)
                TranslationKeys.Add(temp.text);
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

        #endregion

        #region Saving To XML

        XmlDocument document = new XmlDocument();
        XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8",null);
        XmlElement root               = document.DocumentElement;
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

        }
        document.Save(GlobalConsts.PathToSaves+"XX_xx.xml");
        #endregion

    }
}
