using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [SerializeField] private GameObject _eventPanel;
    [SerializeField] private Image      _eventImage;
    [SerializeField] private TMP_Text   _eventText;
    [SerializeField] private TMP_Text   _eventTitle;
    [SerializeField] private Transform  _answerHolder;
    [SerializeField] private GameObject _answerPrefab;
    [SerializeField] private GameObject _blocker;
    [SerializeField] private float      _dissolveSpeed = 0.1f;
    
    private static EventManager     _instance;

    private LazyAnswerLoader   _answerLoader;
    private LazyEventLoader    _eventLoader;
    private LazyResultLoader   _resultLoader;
    private List<AnswerObject> answers;

    private bool          _isDissolve = true;
    private float         _actualStep = 1;
    private bool          _isLaunched = false;
    private Queue<string> _eventQueue;
    private void Start()
    {
        _answerLoader = GetComponent<LazyAnswerLoader>();
        _eventLoader  = GetComponent<LazyEventLoader>();
        _resultLoader = GetComponent<LazyResultLoader>();
        _eventQueue   = new Queue<string>();
    }

    private void Update()
    {
        if (_isDissolve)
        {
            Mathf.Clamp01(_actualStep += _dissolveSpeed);
            _eventImage.material.SetFloat("_DissolveStep", _actualStep);
        }
        else
        {
            Mathf.Clamp01(_actualStep -= _dissolveSpeed);
            _eventImage.material.SetFloat("_DissolveStep", _actualStep);
        }

        if (_eventQueue.Count > 0 && !_isLaunched)
        {
            LaunchEvent(_eventQueue.Dequeue());
        }
    }

    public static EventManager GetInstance()
    {
        if (_instance == null) _instance = FindObjectOfType<EventManager>();
        return _instance;
    }

    public void CloseEvent()
    {
        Clear();   
    }

    private void Clear()
    {
        _eventPanel.SetActive(false);
        _isLaunched        = false;
        _eventImage.sprite = null;
        _isDissolve        = true;
        _actualStep        = 1;
        for (int i = 0; i < _answerHolder.childCount; i++)
        {
            Destroy(_answerHolder.GetChild(i).gameObject);
        }
        _blocker.SetActive(false);

    }

    private bool IsEventPlaying()
    {
        return _isLaunched;
    }

    public void LaunchEvent(string name)
    {
        _blocker.SetActive(true);
        var translator = LanguageManager.GetInstance();
        if (_isLaunched)
        {
            _eventQueue.Enqueue(name);
        }
        else
        {
            SFXManager.GetInstance().Play("EventLaunch");
            _isLaunched = true;
            var eventToLauch = _eventLoader.GetEvent(name);
            if (eventToLauch == null)
            {
                Debug.LogError("Event not found!");
            }
            else
            {
                _isDissolve        = false;
                _eventImage.sprite = eventToLauch._eventSprite;
                _eventText.text    = translator.GetTranslation(eventToLauch._eventDescription);
                _eventTitle.text   = translator.GetTranslation(eventToLauch._eventName);
                answers            = new List<AnswerObject>();
                foreach (var answer in eventToLauch._answerList)
                {
                    var answerToAdd = _answerLoader.GetAnswer(answer);
                    if (answerToAdd != null)
                    {
                        answers.Add(_answerLoader.GetAnswer(answer));
                    }
                    else
                    {
                        Debug.LogError("Answer not found!");
                    }
                }

                foreach (var answer in answers)
                {
                    var child = Instantiate(_answerPrefab, _answerHolder, false);
                    child.GetComponentInChildren<TMP_Text>().text = translator.GetTranslation(answer.AnswerDescription);
                    child.GetComponent<Button>().onClick
                         .AddListener(delegate { AnswerClick(translator.GetTranslation(answer.AnswerDescription)); });
                }

                _eventPanel.SetActive(true);
            }
        }
    }

    public void AnswerClick(string desc)
    {
        var translator = LanguageManager.GetInstance();
        foreach (var answer in answers)
        {
            if (desc.Equals(translator.GetTranslation(answer.AnswerDescription)))
            {
                var result = _resultLoader.GetResult(answer.ResultName);
                if (result != null)
                {
                    result.DoResult(answer.Params);
                }
                else
                {
                    Debug.LogError("Result not found!");
                }
            }
        }
    }
}
