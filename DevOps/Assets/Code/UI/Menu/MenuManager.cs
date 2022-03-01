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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private Button              _newGameButton;
    [SerializeField] private Button              _twitterButton;
    [SerializeField] private Button              _discordButton;
    [SerializeField] private Button              _exitButton;
    [Space] [Header("Settings Tab")]
    [SerializeField] private Button              _settingsButton;
    [SerializeField] private Button              _settingsCloseButton;
    [SerializeField] private Button              _settingsApplyButton;
    [SerializeField] private Button              _qualityUpButton;    
    [SerializeField] private Button              _qualityDownButton;     
    [SerializeField] private Button              _resolutionUpButton;    
    [SerializeField] private Button              _resolutionDownButton;      
    [SerializeField] private Button              _langUpButton;    
    [SerializeField] private Button              _langDownButton;        
    [SerializeField] private Button              _screenUpButton;    
    [SerializeField] private Button              _screenDownButton;    
    [SerializeField] private TMP_Text            _qualityPlaceholder;    
    [SerializeField] private TMP_Text            _resolutionPlaceholder;    
    [SerializeField] private TMP_Text            _langPlaceholder;    
    [SerializeField] private TMP_Text            _screenPlaceholder;    
    [SerializeField] private GameObject          _settingsPanel;
    [SerializeField] private Slider              _sfxVolumeSlider;
    [SerializeField] private Slider              _musicVolumeSlider;
    [SerializeField] private AudioMixer          _mixer;
    [SerializeField] private TMP_InputField      _reportBugInputField;
    [SerializeField] private Button              _reportBugButton;
    
    private                  int                 _qualityIterator    = 2;
    private                  int                 _resolutionIterator = 8;
    private                  int                 _screenIterator     = 0;
    private                  int                 _langIterator       = 0;
    #region Settings Const Arrays

    public static readonly string[] LIST_OF_QUALITIES = new []
    {
        "Very_Low_Quality_Settings", "Low_Quality_Settings", "Medium_Quality_Settings", "High_Quality_Settings", "Very_High_Quality_Settings", "Ultra_Quality_Settings"
    };
    public static readonly string[] LIST_OF_RESOLUTIONS = new []
    {
        "800x600", "1024x768", "1280x1024", "1440x900", "1680x1050", "1920x1200", "1280x720", "1366x768", "1920x1080"
    };
    public static readonly string[] LIST_OF_FULLSCREEN_OPTIONS = new []
    {
        "Full_Screen_Settings", "Windowed_Screen_Settings", "Windowed_Maximized_Screen_Settings", "Exclusive_Windowed_Screen_Settings"
    };
    public static readonly string[] LIST_OF_LANGUAGES_OPTIONS = new []
    {
        "English_Lang_Settings", "Polish_Lang_Settings", "Germany_Lang_Settings", "Simplified_Chinese_Lang_Setting", "Portugal_Lang_Setting", "Spanish_Lang_Setting"
    };

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        //GraphicsSettings.defaultRenderPipeline = _mediumRenderPipelineAsset;
        //QualitySettings.renderPipeline         = _mediumRenderPipelineAsset;
        LoadSettings();
        UpdatePlaceholders();
        #region Button Bindings

        _newGameButton.onClick.AddListener(StartNewGame);
        _discordButton.onClick.AddListener(()=>Application.OpenURL(GlobalConsts.DiscordLink));
        _twitterButton.onClick.AddListener(()=>Application.OpenURL(GlobalConsts.TwitterLink));
        _exitButton.onClick.AddListener(()=>Application.Quit());
        _settingsButton.onClick.AddListener(delegate
        {
            _settingsPanel.SetActive(true);
        });
        _settingsCloseButton.onClick.AddListener(delegate
        {
            _settingsPanel.SetActive(true);
        });
        _qualityUpButton.onClick.AddListener(delegate
        {
            Mathf.Clamp(_qualityIterator++, 0, LIST_OF_QUALITIES.Length - 1);
            UpdatePlaceholders();
        });
        _qualityDownButton.onClick.AddListener(delegate
        {
            Mathf.Clamp(_qualityIterator--, 0, LIST_OF_QUALITIES.Length - 1);
            UpdatePlaceholders();

        });
        _resolutionUpButton.onClick.AddListener(delegate
        {
            Mathf.Clamp(_resolutionIterator++, 0,
                        LIST_OF_RESOLUTIONS.Length - 1);
           UpdatePlaceholders();
            
        });
        _resolutionDownButton.onClick.AddListener(delegate
        {
            Mathf.Clamp(_resolutionIterator--, 0,
                        LIST_OF_RESOLUTIONS.Length - 1);
            UpdatePlaceholders();
        });
        _langUpButton.onClick.AddListener(delegate
        {
            Mathf.Clamp(_langIterator++, 0, LIST_OF_LANGUAGES_OPTIONS.Length - 1);
            UpdatePlaceholders();
        });
        _langDownButton.onClick.AddListener(delegate
        {
            Mathf.Clamp(_langIterator--, 0, LIST_OF_LANGUAGES_OPTIONS.Length - 1);
            UpdatePlaceholders();
        });
        _screenUpButton.onClick.AddListener(delegate
        {
            Mathf.Clamp(_screenIterator++, 0,
                        LIST_OF_FULLSCREEN_OPTIONS.Length - 1);
            UpdatePlaceholders();
        });
        _screenDownButton.onClick.AddListener(delegate
        {
            Mathf.Clamp(_screenIterator--, 0,
                        LIST_OF_FULLSCREEN_OPTIONS.Length - 1);
            UpdatePlaceholders();
        });
        _musicVolumeSlider.onValueChanged.AddListener(delegate(float value)
        {
            _mixer.SetFloat(GlobalConsts.MixerMusicProperyName, Mathf.Log10(value) * 20);
        });
        _sfxVolumeSlider.onValueChanged.AddListener(delegate(float value)
        {
            _mixer.SetFloat(GlobalConsts.MixerSFXProperyName, Mathf.Log10(value) * 20);
        });
        _reportBugButton.onClick.AddListener(()=>SendBugEmail());
        _settingsApplyButton.onClick.AddListener(ApplySetting);
        #endregion
    }

    private void LoadSettings()
    {
        if (File.Exists(GlobalConsts.PathToSettings))
        {
            using var     stream     = File.Open(GlobalConsts.PathToSettings, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(SettingsSaveData));
            var loadData = (SettingsSaveData) serializer.Deserialize(stream);
            _qualityIterator         = loadData.QualityIterator;
            _resolutionIterator      = loadData.ResolutionIterator;
            _langIterator            = loadData.LangIterator;
            _screenIterator          = loadData.ScreenIterator;
            _sfxVolumeSlider.value   = loadData.SFXVolume;
            _musicVolumeSlider.value = loadData.MusicVolume;
        }
        ApplySetting();
    }

    private void UpdatePlaceholders()
    {
        _screenPlaceholder.text =
            LanguageManager.GetInstance().GetTranslation(LIST_OF_FULLSCREEN_OPTIONS[_screenIterator]);
        _langPlaceholder.text =
            LanguageManager.GetInstance().GetTranslation(LIST_OF_LANGUAGES_OPTIONS[_langIterator]);
        _resolutionPlaceholder.text =
            LanguageManager.GetInstance().GetTranslation(LIST_OF_RESOLUTIONS[_resolutionIterator]);
        _qualityPlaceholder.text =
            LanguageManager.GetInstance().GetTranslation(LIST_OF_QUALITIES[_qualityIterator]);
    }

    private void StartNewGame()
    {
        PlayerPrefs.SetInt("NewGame", 1);
        SceneManager.LoadScene("Scenes/Mapa");
    }
    
    private void ApplySetting()
    {
        switch (_qualityIterator)
        {
            /*case 0:
                QualitySettings.renderPipeline = _veryLowRenderPipelineAsset;
                break;
            case 1:
                QualitySettings.renderPipeline = _lowRenderPipelineAsset;
                break;
            case 2:
                QualitySettings.renderPipeline = _mediumRenderPipelineAsset;
                break;
            case 3:
                QualitySettings.renderPipeline = _highRenderPipelineAsset;
                break;
            case 4:
                QualitySettings.renderPipeline = _veryHighRenderPipelineAsset;
                break;
            case 5:
                QualitySettings.renderPipeline = _ultraRenderPipelineAsset;
                break;*/
        }
        LanguageManager.GetInstance().SetLang(_langIterator);
        FullScreenMode screenMode;
        switch (_screenIterator)
        {
            case 0:
                screenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                screenMode = FullScreenMode.Windowed;
                break;
            case 2:
                screenMode = FullScreenMode.MaximizedWindow;
                break;
            case 3:
                screenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            default:
                screenMode = FullScreenMode.FullScreenWindow;
                break;
        }
        var            resolution = LIST_OF_RESOLUTIONS[_resolutionIterator].Split('x');
        int            width, height;
        if (int.TryParse(resolution[0], out width) && int.TryParse(resolution[1], out height))
        {
            Screen.SetResolution(width,height,screenMode);
        }

        SettingsSaveData saveData   = new SettingsSaveData
        {
            QualityIterator = _qualityIterator,
            LangIterator =  _langIterator,
            ScreenIterator = _screenIterator,
            ResolutionIterator = _resolutionIterator,
            MusicVolume = _musicVolumeSlider.value,
            SFXVolume =  _sfxVolumeSlider.value
        };
        using var     stream     = File.Open(GlobalConsts.PathToSettings, FileMode.Create);
        XmlSerializer serializer = new XmlSerializer(typeof(SettingsSaveData));
        serializer.Serialize(stream, saveData);
    }

    private void SendBugEmail()
    {
        try
        {
            SmtpClient client = new SmtpClient(GlobalConsts.EmailHost, GlobalConsts.EmailPort);
            client.Credentials = new NetworkCredential(GlobalConsts.EmailUser, GlobalConsts.EmailPassword);
            client.EnableSsl   = true;
            MailAddress from    = new MailAddress(GlobalConsts.EmailUser, GlobalConsts.EmailName, Encoding.UTF8);
            MailAddress to      = new MailAddress(GlobalConsts.EmailDest);
            MailMessage message = new MailMessage(from, to);
            message.Body            =  _reportBugInputField.text;
            message.BodyEncoding    =  Encoding.UTF8;
            var guidG = Guid.NewGuid().ToString();
            message.Subject         =  $"Bug Report #{guidG}";
            message.SubjectEncoding =  Encoding.UTF8;
            client.SendCompleted    += new SendCompletedEventHandler(SendCompletedCallback);
            string userState = "test message1";
            client.SendAsync(message, userState);
        }
        catch (Exception ep)
        {
            Debug.LogError(ep);
        }
    }
    private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        // Get the unique identifier for this asynchronous operation.
        string token = (string)e.UserState;
        if (e.Error != null)
        {
            Debug.LogError("[ " +token +" ] " + " " + e.Error.ToString());
        }
    }
    [Serializable][XmlRoot("Settings")]
    public struct SettingsSaveData
    {
        [XmlElement(ElementName = "Quality")]
        public int QualityIterator;
        [XmlElement(ElementName = "Lang")]
        public int LangIterator;
        [XmlElement(ElementName = "Screen Mode")]
        public int ScreenIterator;
        [XmlElement(ElementName = "Resolution")]
        public int ResolutionIterator;
        [XmlElement(ElementName = "SFX Volume")]
        public float SFXVolume;
        [XmlElement(ElementName = "Music Volume")]
        public float MusicVolume;
    }
}
