using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private int         _hoursPerDay             = 6;  //Ile godzin ma doba
    [SerializeField] private int         _minutesPerDayMultipiler = 60; //mnożnik minut dla doby
    [SerializeField] private TMP_Text _text;
    private                  int         _minutesPerDay; //ile minut trwa doba
    private                  int         _time = 0;      //czas w minutach od północy
    private                  int         _day  = 1;      //który dzień od początku rozgrywki (numeracja od 1)
    
    // Start is called before the first frame update
    void Start()
    {
        _minutesPerDay = _hoursPerDay * _minutesPerDayMultipiler;
        PassTime(60);
        _text.text     = GetTimeAsString();
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
}
