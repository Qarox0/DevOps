using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Code.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

public class GameMenuController : MonoBehaviour
{
    #region Menu Variables
    [SerializeField] private GameObject          _menuPanel;
    [SerializeField] private GameObject          _confirmationPanel;
    [SerializeField] private GameObject          _savePanel;
    [SerializeField] private GameObject          _loadPanel;
    [SerializeField] private Button              _openGameMenuButton;
    [SerializeField] private Button              _closeGameMenuButton;
    [SerializeField] private Button              _closeSavePanelButton;
    [SerializeField] private Button              _closeLoadPanelButton;
    [SerializeField] private Button              _exitGameButton;
    [SerializeField] private Button              _confirmationYesButton;
    [SerializeField] private Button              _confirmationNoButton;
    [SerializeField] private TMP_Text            _confirmationText;
    [SerializeField] private Button              _newGameGameMenuButton;
    [SerializeField] private Button              _saveGameMenuButton;
    [SerializeField] private Button              _loadGameMenuButton;
    [SerializeField] private Button              _settingGameMenuButton;
    [SerializeField] private GameObject          _loadContent;
    [SerializeField] private GameObject          _saveContent;
    [SerializeField] private Button              _saveGameButton;
    [SerializeField] private Button              _loadGameButton;
    [SerializeField] private TMP_InputField      _saveNameInputField;
    [SerializeField] private TMP_Text            _saveMenuTitleText;
    [SerializeField] private TMP_Text            _saveNamePlaceholderText;
    [SerializeField] private GameObject          _saveLoadPrefab;
    [SerializeField] private GameObject          _blocker;
    [SerializeField] private PlayerInput         _input;
    
    //Debug 
    [SerializeField] private MapDefinitionObject _map;
    [SerializeField] private Button              _startAdventureButton;
    [SerializeField] private Button              _returnAdventureButton;
    [SerializeField] private Button              _toggleBackpackButton;
    [SerializeField] private Button              _toggleCraftingButton;
    [SerializeField] private Button              _doButton;
    [SerializeField] private Image               _transitionPanel;
    [SerializeField] private GameObject          _backpackPanel;
    [SerializeField] private GameObject          _craftingPanel;
    private                  bool                _onAdventure = false;
    #endregion

    private string fileName;
    // Start is called before the first frame update
    void Start()
    {
        #region Button Bindings

        _openGameMenuButton.onClick.AddListener(OpenMenu);
        _closeGameMenuButton.onClick.AddListener(CloseMenu);
        _exitGameButton.onClick.AddListener(() => OpenConfirmation(ConfirmationContextEnum.EXIT));
        _confirmationNoButton.onClick.AddListener(CloseConfirmation);
        _newGameGameMenuButton.onClick.AddListener(() => OpenConfirmation(ConfirmationContextEnum.NEW_GAME));
        _saveGameMenuButton.onClick.AddListener(OpenSaveMenu);
        _loadGameMenuButton.onClick.AddListener(OpenLoadMenu);
        _settingGameMenuButton.onClick.AddListener(OpenSettingsMenu);
        _closeSavePanelButton.onClick.AddListener(CloseSaveMenu);
        _closeLoadPanelButton.onClick.AddListener(CloseLoadMenu);
        _loadGameButton.onClick.AddListener(LoadGame);
        _saveGameButton.onClick.AddListener(SaveGame);
        //debug
        _startAdventureButton.onClick.AddListener(StartAdventure);
        _returnAdventureButton.onClick.AddListener(ReturnAdventure);
        _toggleBackpackButton.onClick.AddListener(()=> _backpackPanel.SetActive(!_backpackPanel.activeSelf));
        _toggleCraftingButton.onClick.AddListener(()=> _craftingPanel.SetActive(!_craftingPanel.activeSelf));
        _doButton.onClick.AddListener(()=> _input.transform.parent.GetComponent<HexScript>().HandlePlayerInteraction(_input.GetComponent<Player>()));
        #endregion

        if (_input == null) _input = FindObjectOfType<PlayerInput>();
    }

    #region Button Methods

    private void StartAdventure()
    {
        if (!_onAdventure)
        {
            _onAdventure = true;
            ScreenOff();
        }
    }
    private void ReturnAdventure()
    {
        if (_onAdventure)
        {
            _onAdventure = false;
            ScreenOffR();
        }
    }
    //Debug
    private void ScreenOff()
    {
        _transitionPanel.gameObject.SetActive(true);
        _transitionPanel.DOColor(new Color(0, 0, 0, 1), 3).OnComplete(Load);
    }
    private void ScreenOffR()
    {
        _transitionPanel.gameObject.SetActive(true);
        _transitionPanel.DOColor(new Color(0, 0, 0, 1), 3).OnComplete(LoadR);
    }

    private void LoadR()
    {
        MapManager.GetInstance().ReturnPlayer(_input.gameObject);
        _transitionPanel.DOColor(new Color(0, 0, 0, 0), 3).OnComplete(ScreenOn);
    }
    private void Load()
    {
        MapManager.GetInstance().LoadLevelFromCurrentData(_input.gameObject, _map);
        _transitionPanel.DOColor(new Color(0, 0, 0, 0), 3).OnComplete(ScreenOn);
    }
    private void ScreenOn()
    { 
        _transitionPanel.gameObject.SetActive(false);
    } 
    //Debug End
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
        for (int i = 0; i < _saveContent.transform.childCount; i++)
        {
            Destroy(_saveContent.transform.GetChild(i).gameObject);
        }
        foreach (var file in Directory.EnumerateFiles(GlobalConsts.PathToSaves))
        {
            string[] saveNameSplit = file.Split('/');
            string   saveName      = saveNameSplit[saveNameSplit.Length - 1].Split('.')[0];
            var      child         = Instantiate(_saveLoadPrefab, _loadContent.transform, false);
            child.transform.GetChild(0).GetComponent<TMP_Text>().text = saveName;
            child.GetComponent<Button>().onClick.AddListener(() => fileName = saveName);
            var child2 = Instantiate(_saveLoadPrefab,_saveContent.transform,false);
            child2.transform.GetChild(0).GetComponent<TMP_Text>().text = saveName;
            child2.GetComponent<Button>().onClick.AddListener(() => fileName = saveName);

        }
    }
    private void OpenConfirmation(ConfirmationContextEnum context)
    {
        _confirmationPanel.SetActive(true);
        switch (context)
        {
            case ConfirmationContextEnum.EXIT:
                _confirmationYesButton.onClick.RemoveAllListeners();
                _confirmationYesButton.onClick.AddListener(ExitGame);
                break;
            case ConfirmationContextEnum.NEW_GAME:
                Debug.Log("New Game Implementation");
                break;
            case ConfirmationContextEnum.OVERWRITE:
                Debug.Log("Overwrite save implementation");
                break;
        }
    }

    private void LoadGame()
    {
        if (File.Exists($"{GlobalConsts.PathToSaves}{fileName}{GlobalConsts.SaveFileExtension}"))
        {
            SLAM.GetInstance().Load($"{GlobalConsts.PathToSaves}{fileName}{GlobalConsts.SaveFileExtension}");
            Debug.Log("Loaded");
        }
        else
        {
            Debug.Log($"File not exists at: {GlobalConsts.PathToSaves}{fileName}");
        }
    }

    private void SaveGame()
    {
        fileName = _saveNameInputField.text;
        if(fileName != String.Empty)
        {
            SLAM.GetInstance().Save($"{GlobalConsts.PathToSaves}{fileName}{GlobalConsts.SaveFileExtension}");
            LoadSaves();
        }
    }

    private void CloseConfirmation()
    {
        _confirmationPanel.SetActive(false);
    }

    private void OpenMenu()
    {
        _menuPanel.SetActive(true);
        _blocker.SetActive(true);
        _input.actions.Disable();
        
    }

    private void CloseMenu()
    {
        _menuPanel.SetActive(false);
        _blocker.SetActive(false);
        _input.actions.Enable();
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void OpenSaveMenu()
    {
        _savePanel.SetActive(true);        
        LoadSaves();
    }

    private void CloseSaveMenu()
    {
        _savePanel.SetActive(false);
    }

    private void OpenLoadMenu()
    {
        _loadPanel.SetActive(true);
        LoadSaves();
    }

    private void CloseLoadMenu()
    {
        _loadPanel.SetActive(false);
    }
    
    private void OpenSettingsMenu()
    {
        Debug.Log("Settings not implemented yet");
    }
    
    private void CloseSettingsMenu()
    {
        Debug.Log("Settings not implemented yet");
    }

    #endregion
    // Update is called once per frame
    void Update()
    {
        
    }
}
