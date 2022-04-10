using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [SerializeField] private List<Weather> PossibleWeather;
    [Range(0,99)]
    [SerializeField] private int           _maxWeatherMultiplier = 1;
    [Range(0,99)]
    [SerializeField] private int           _minWeatherMultiplier = 0;
    [SerializeField] private Animator      _weatherAnimator;
    private                  Weather       _actualWeather;
    private                  int           _duration;
#pragma warning disable CS0414
    private                  bool          _locked = false;
#pragma warning restore CS0414
    // Start is called before the first frame update
    void Start()
    {
        TimeManager.GetTimeManagerInstance().onTimePasses += CalculateWeather;
    }

    private void CalculateWeather(int time)
    {
        _duration -= time;
        if (_duration <= 0)
        {
            _weatherAnimator.SetBool(_actualWeather.Type.ToString(), false);
            int i = 0;
            while (_duration <= 0 && i < 999)
            {
                foreach (var weather in PossibleWeather)
                {
                    float roll = Random.Range(0f, 1f);
                    if (roll <= weather.MaxOccurenceChance && roll >= weather.MinOccurenceChance)
                    {
                        _actualWeather =  weather;
                        _duration      = weather.MinimalDuration * Random.Range(_minWeatherMultiplier, _maxWeatherMultiplier);
                        _weatherAnimator.SetBool(weather.Type.ToString(), true);
                        break;
                    }
                }
                i++;
            }
        }
    }
    
}
