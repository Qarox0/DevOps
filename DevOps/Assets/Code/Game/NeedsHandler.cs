using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeedsHandler : MonoBehaviour
{
    [SerializeField] private float  _defaultHungerDrop = 1;
    [SerializeField] private float  _defaultThirstDrop = 1;
    private                  Player _player;

    [SerializeField] private Image _thirstImage;
    [SerializeField] private Image _hungerImage;
    [SerializeField] private Image _healthImage;

    private static NeedsHandler _instance;

    public static NeedsHandler GetInstance()
    {
        if (_instance == null) _instance = FindObjectOfType<NeedsHandler>();
        return _instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        _player                                           =  FindObjectOfType<Player>();
        TimeManager.GetTimeManagerInstance().onTimePasses += LowerNeeds;
        
    }

    public void RestoreNeeds(float hunger, float thirst)
    {
        _player.stats.Hunger  += hunger;
        _player.stats.Thirsty += thirst;
        UpdateVisualStatus();
    }

    private void LowerNeeds(int amountOfTimePassed)
    {
        float healthDamage = 0;
        if (_player.stats.Hunger < amountOfTimePassed * _defaultHungerDrop)
        {
            healthDamage         =  amountOfTimePassed * _defaultHungerDrop - _player.stats.Hunger;
            _player.stats.Hunger =  0;
            _player.stats.Health -= healthDamage;
        }
        else
        {
            _player.stats.Hunger -= amountOfTimePassed * _defaultHungerDrop;
        }
        if (_player.stats.Thirsty < amountOfTimePassed * _defaultThirstDrop)
        {
            healthDamage         =  amountOfTimePassed * _defaultThirstDrop - _player.stats.Thirsty;
            _player.stats.Thirsty =  0;
            _player.stats.Health -= healthDamage;
        }
        else
        {
            _player.stats.Thirsty -= amountOfTimePassed * _defaultThirstDrop;
        }
        UpdateVisualStatus();
    }

    private void UpdateVisualStatus()
    {
        _thirstImage.color = new Color(1 , 0 +_player.stats.Thirsty / 100, 0 +_player.stats.Thirsty / 100, 1);
        _hungerImage.color = new Color(1, 0 +_player.stats.Thirsty / 100, 0 +_player.stats.Thirsty / 100, 1);
        _healthImage.color = new Color(1 , 1, 1, _player.stats.Health / 100);
    }
    


}
