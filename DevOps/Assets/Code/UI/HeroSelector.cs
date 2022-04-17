using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeroSelector : MonoBehaviour
{
    [SerializeField] private List<HeroObject> _listOfHeroes;
    [SerializeField] private GameObject       _content;
    [SerializeField] private GameObject       _heroSelectPrefab;
    [SerializeField] private Image            _heroView;
    [SerializeField] private TMP_Text         _heroTitle;
    [SerializeField] private TMP_Text         _heroBonuses;
    [SerializeField] private TMP_Text         _heroDescription;
    [SerializeField] private Button           _accept;
    private                  bool             _heroSelected = false;

    private void Start()
    {
        foreach (var hero in _listOfHeroes)
        {
            CreateHeroButton(hero.HeroImage, hero.Name, hero.StatsBonuses, hero.Description, hero.HeroDefName);
        }

        _accept.onClick.AddListener(() =>
        {
            if (_heroSelected)
            {
                SceneManager.LoadScene("Scenes/Mapa");
            }
        });
    }

    private void CreateHeroButton(Sprite heroImage, string title, List<StatsBonus> bonusList, string description, string heroDefName)
    {
        var child = Instantiate(_heroSelectPrefab, _content.transform, false);
        child.GetComponentInChildren<Image>().sprite = heroImage;
        child.GetComponent<Button>().onClick.AddListener(() =>
        {
            _heroView.sprite      = heroImage;
            _heroTitle.text       = title;
            _heroDescription.text = description;
            _heroBonuses.text     = "";
            foreach (var bonus in bonusList)
            {
                if (bonus.Positive == true)
                {
                    _heroBonuses.text += $"<color=\"green\">{bonus.Name}: +{bonus.value}</color>";
                }
                else
                {
                    _heroBonuses.text += $"<color=\"red\">{bonus.Name}: {bonus.value}</color>";
                }

                _heroBonuses.text += "\u000a";
            }

            _heroSelected = true;
            PlayerPrefs.SetString("PlayerSelected", heroDefName);
        });
    }
}
