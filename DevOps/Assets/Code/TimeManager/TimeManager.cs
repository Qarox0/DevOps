using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TimeManager : MonoBehaviour, ISaveable
{
    public event Action<int> onTimePasses;
    
    [SerializeField] private int      _hoursPerDay              = 6;  //Ile godzin ma doba
    [SerializeField] private int      _minutesPerHourMultipiler = 60; //mnożnik minut dla doby
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image    _transitionPanel;
    private                  int      _minutesPerDay; //ile minut trwa doba
    private                  int      _time = 0;      //czas w minutach od północy
    private                  int      _day  = 1;      //który dzień od początku rozgrywki (numeracja od 1)

    //TODO Wyrzucić do osobnej klasy
    [SerializeField][Range(0,1)]
    private float _bloodNightStartTime = 0.75f;
    [SerializeField][Range(0,100)]
    private int _bloodNightStartChance = 100;

    private bool _bloodNightRolled = false;
    // Start is called before the first frame update
    void Start()
    {
        _minutesPerDay = _hoursPerDay * _minutesPerHourMultipiler;
        PassTime(60);
        _text.text   =  GetTimeAsString();
        onTimePasses += HandleTimeEvents;
    }

    public void HandleTimeEvents(int time)
    {
        if (_time > _minutesPerDay * _bloodNightStartTime && !_bloodNightRolled)
        {
            if (Random.Range(0, 100) <= _bloodNightStartChance)
            {
                EventManager.GetInstance().LaunchEvent("BloodNightEvent");
            }

            _bloodNightRolled = true;
        }

        if (_time < 20)
        {
            _bloodNightRolled = false;
        }
    }
    private static TimeManager _instance;            //instancja time menadżera
    public static TimeManager GetTimeManagerInstance() //Singleton Time Menadżera
    {
        if (_instance == null) _instance = FindObjectOfType<TimeManager>(); //Znajdź Instancje tm w hierarchii
        return _instance;                                                 //zwróć instancje
    }

    public void PassTime(int minutesToPass)
    {
        _time += minutesToPass;
        if (_time >= _minutesPerDay)
        {
            _time -= _minutesPerDay;
            _day++;
        }
        _text.text = GetTimeAsString();
        onTimePasses?.Invoke(minutesToPass);
    }

    public string GetTimeAsString() //Wyświetlanie zegara
    {
        int hours = _time / 60;
        int minutes = _time % 60;

        if (minutes < 10) return $"{hours.ToString()}:0{minutes.ToString()}";
        else return $"{hours.ToString()}:{minutes.ToString()}";
    }

    public int GetTimeInMinutes()
    {
        return _time;
    }

    public int GetDayNumber()
    {
        return _day;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public object CaptureState()
    {
        throw new NotImplementedException();
    }

    public void   RestoreState(object state)
    {
        throw new NotImplementedException();
    }
    public void Sleep(int sleepTime)
    {
        _transitionPanel.gameObject.SetActive(true);
        _transitionPanel.DOColor(new Color(0, 0, 0, 1), 3).OnComplete(()=>
        {
            _transitionPanel.DOColor(new Color(0, 0, 0, 0), 3);
            PassTime(sleepTime);
        });
        
    }
    
    public struct TimeSaveData
    {
        public int Time;
        public int Day;
    }
    
}
