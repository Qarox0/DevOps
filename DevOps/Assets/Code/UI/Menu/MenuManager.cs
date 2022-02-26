using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _twitterButton;
    [SerializeField] private Button _discordButton;
    [SerializeField] private Button _exitButton;
    // Start is called before the first frame update
    void Start()
    {
        _newGameButton.onClick.AddListener(StartNewGame);
        _discordButton.onClick.AddListener(OpenDiscord);
        _twitterButton.onClick.AddListener(OpenTwiter);
        _exitButton.onClick.AddListener(ExitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartNewGame()
    {
        PlayerPrefs.SetInt("NewGame", 1);
        SceneManager.LoadScene("Scenes/Mapa");
    }

    private void OpenTwiter()
    {
        Debug.LogError("Not linked yet");
        Application.OpenURL("http://unity3d.com/");
    }
    
    private void OpenDiscord()
    {
        Application.OpenURL("https://discord.gg/xKBvaAKU");
    }
    
    private void ExitGame()
    {
        Application.Quit();
    }

}
