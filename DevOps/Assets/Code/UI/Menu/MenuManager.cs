using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Xml.Serialization;
using Code.Utils;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private Button     _newGameButton;
    [SerializeField] private Button               _loadGameButton;
    [SerializeField] private Button               _twitterButton;
    [SerializeField] private Button               _discordButton;
    [SerializeField] private Button               _exitButton;
    [SerializeField] private GameObject           _blocker;
    [SerializeField] private SettingsPanelManager _settingsManager;
    
    
    [Space] [Header("Save&Load System")] 
    [SerializeField] private GameObject     _loadPanel;
    [SerializeField] private Button     _loadButton;
    [SerializeField] private Button     _closeLoadPanelButton;
    [SerializeField] private GameObject _loadContent;
    [SerializeField] private GameObject _loadPrefab;
    private                  string     fileName;
    
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        _settingsManager.LoadSettings();
        #region Button Bindings

        _newGameButton.onClick.AddListener(StartNewGame);
        _discordButton.onClick.AddListener(()=>Application.OpenURL(GlobalConsts.DiscordLink));
        _twitterButton.onClick.AddListener(()=>Application.OpenURL(GlobalConsts.TwitterLink));
        _exitButton.onClick.AddListener(()=>Application.Quit());
        
        _loadGameButton.onClick.AddListener(OpenLoadGamePanel);
        _closeLoadPanelButton.onClick.AddListener(CloseLoadGamePanel);
        _loadButton.onClick.AddListener(LoadGame);
        #endregion
    }

    private void OpenLoadGamePanel()
    {
        _loadPanel.SetActive(true);
        _blocker.SetActive(true);
        LoadSaves();
    }
    private void CloseLoadGamePanel()
    {
        _loadPanel.SetActive(false);
        _blocker.SetActive(false);
    }

    
    private void LoadSaves()
    {
        if(!Directory.Exists(GlobalConsts.PathToSaves))
        {
            Directory.CreateDirectory(GlobalConsts.PathToSaves);
        }
        for (int i = 0; i < _loadContent.transform.childCount; i++)
        {
            Destroy(_loadContent.transform.GetChild(i).gameObject);
        }
        foreach (var file in Directory.EnumerateFiles(GlobalConsts.PathToSaves))
        {
            string[] saveNameSplit = file.Split('/');
            string   saveName      = saveNameSplit[saveNameSplit.Length - 1].Split('.')[0];
            var      child         = Instantiate(_loadPrefab, _loadContent.transform, false);
            child.transform.GetChild(0).GetComponent<TMP_Text>().text = saveName;
            child.GetComponent<Button>().onClick.AddListener(() => fileName = saveName);
        }
    }
    private void LoadGame()
    {
        
        if (File.Exists($"{GlobalConsts.PathToSaves}{fileName}{GlobalConsts.SaveFileExtension}"))
        {
            PlayerPrefs.SetInt("NewGame", 0);
            PlayerPrefs.SetString("GameToLoad", fileName);
            SceneManager.LoadScene("Scenes/Mapa");
        }
    }

    

    private void StartNewGame()
    {
        PlayerPrefs.SetInt("NewGame", 1);
        SceneManager.LoadScene("Scenes/Mapa");
    }
    
    

    
    
}
