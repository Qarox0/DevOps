using System;
using System.Collections;
using System.Collections.Generic;
using Code.Utils;
using DG.Tweening;
using OK.StatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Player : MonoBehaviour, ISaveable
{
    public event Action onPlayerMove;

    public PlayerStats stats;
    
    [SerializeField] private GameObject     _inventoryHandle; //uchwyt do ui inv
    [SerializeField] private GameObject     _craftingHandle;  
    [SerializeField] private GameObject     _buildingHandle;  
    [SerializeField] private GameObject     _blocker;         
    [SerializeField] private GameObject     _narratorBlockPrefab;
    [SerializeField] private GameObject     _worldSpaceCanvas;
    [SerializeField] private int            _timeTakenToMove; //Czas potrzebny na przejscie pola
    private                  SpriteRenderer _renderer;
    private                  int            _stepCounter = 0;
    private                  float          _actionDelay = 0;
    private                  string         _heroDef;

    private static Player _instance;

    public static Player GetInstance()
    {
        if (_instance == null) _instance = FindObjectOfType<Player>();
        return _instance;
    }

    /*public void DealDamage(BodyPart part, int amount)
    {
        switch (part)
        {
            
        }

        UpdatePartsVisual();
    }*/

    private void UpdatePartsVisual()
    {
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("PlayerSelected"))
        {
            _heroDef = PlayerPrefs.GetString("PlayerSelected");
        }
        _renderer    =  GetComponent<SpriteRenderer>();
        LoadPlayer();
        onPlayerMove += HandleMoveEvents;
    }

    private void LoadPlayer()
    {
        HeroObject hero = null;
        hero = Resources.Load<HeroObject>($"{GlobalConsts.PathToHeroes}{_heroDef}");

        if (hero != null)
        {
            InitializeStats(hero.StatsBonuses);
            _renderer.sprite = hero.HeroImage; 
        }
        else
        {
            InitializeStats();
        }
    }

    public void Equip(EquipStatsObject stats)
    {
        foreach (var modifier in stats.Strength)
        {
            this.stats.Strength.AddModifier( modifier);
        }
        foreach (var modifier in stats.Toughness)
        {
            this.stats.Toughness.AddModifier( modifier);
        }
        foreach (var modifier in stats.Reflex)
        {
            this.stats.Reflex.AddModifier( modifier);
        }
        foreach (var modifier in stats.Intelligence)
        {
            this.stats.Intelligence.AddModifier( modifier);
        }
        foreach (var modifier in stats.Karma)
        {
            this.stats.Karma.AddModifier( modifier);
        }
        foreach (var modifier in stats.Exactitude)
        {
            this.stats.Exactitude.AddModifier( modifier);
        }
        foreach (var modifier in stats.MaxHealth)
        {
            this.stats.MaxHealth.AddModifier( modifier);
        }
        foreach (var modifier in stats.MaxWeight)
        {
            this.stats.MaxWeight.AddModifier( modifier);
        }
        foreach (var modifier in stats.MaxHunger)
        {
            this.stats.MaxHunger.AddModifier( modifier);
        }
        foreach (var modifier in stats.MaxThirst)
        {
            this.stats.MaxThirst.AddModifier( modifier);
        }
    }
    public void Unequip(EquipStatsObject stats)
    {
        foreach (var modifier in stats.Strength)
        {
            this.stats.Strength.RemoveModifier( modifier);
        }
        foreach (var modifier in stats.Toughness)
        {
            this.stats.Toughness.RemoveModifier( modifier);
        }
        foreach (var modifier in stats.Reflex)
        {
            this.stats.Reflex.RemoveModifier( modifier);
        }
        foreach (var modifier in stats.Intelligence)
        {
            this.stats.Intelligence.RemoveModifier( modifier);
        }
        foreach (var modifier in stats.Karma)
        {
            this.stats.Karma.RemoveModifier( modifier);
        }
        foreach (var modifier in stats.Exactitude)
        {
            this.stats.Exactitude.RemoveModifier( modifier);
        }
        foreach (var modifier in stats.MaxHealth)
        {
            this.stats.MaxHealth.RemoveModifier( modifier);
        }
        foreach (var modifier in stats.MaxWeight)
        {
            this.stats.MaxWeight.RemoveModifier( modifier);
        }
        foreach (var modifier in stats.MaxHunger)
        {
            this.stats.MaxHunger.RemoveModifier( modifier);
        }
        foreach (var modifier in stats.MaxThirst)
        {
            this.stats.MaxThirst.RemoveModifier( modifier);
        }
    }
    

    private void InitializeStats(List<StatsBonus> bonuses = null)
    {
        stats              = new PlayerStats();
        stats.Strength = new SAttribute(5);
        stats.Toughness = new SAttribute(5);
        stats.Intelligence = new SAttribute(5);
        stats.Karma = new SAttribute(5);
        stats.Exactitude = new SAttribute(5);
        stats.MaxHunger = new SAttribute(100);
        stats.MaxWeight = new SAttribute(100);
        stats.MaxHealth = new SAttribute(100);
        stats.MaxThirst = new SAttribute(100);
        
    }

    private void HandleMoveEvents()
    {
        if (_stepCounter == 100)
        {
            EventManager.GetInstance().LaunchEvent("PlanetPioneerEvent");
        }
    }

    public void InteractWithHexBelow(InputAction.CallbackContext value) //input interakcji z hexem na którym stoimy
    {
        if (value.started && _blocker.activeSelf == false && _actionDelay <= 0)
        {
            transform.parent.GetComponent<HexScript>().HandlePlayerInteraction(this);
            _actionDelay += 1;
        }
    }

    public void Fishing(InputAction.CallbackContext value)
    {
        if (value.started && _blocker.activeSelf == false)
        {
            var fishingSpot = transform.parent.GetComponent<HexScript>().GetFishingSpot();
            if (fishingSpot != null)
            {
                fishingSpot.GetComponentInChildren<FishableHex>().Interaction(this);
            }
        }
    }

    public void MovePlayer(InputAction.CallbackContext value)               //input poruszania się
    {
        if (value.started && _blocker.activeSelf == false)
        {
            var          mousePosition = Mouse.current.position.ReadValue();
            Vector2      inWorldSpace  = Camera.main.ScreenToWorldPoint(mousePosition);
            RaycastHit2D hit           = Physics2D.Raycast(inWorldSpace, Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "Hex" && hit.collider.gameObject != transform.parent.gameObject)
            {
                var hex         = transform.parent.GetComponent<HexScript>();
                var objectOnHex = hit.collider.GetComponentInChildren<IHexable>();
                if (transform.parent.GetComponent<HexScript>().IsAdjecent(hit.collider.gameObject))
                {
                    if (objectOnHex != null && objectOnHex.IsPassable)
                    {
                        transform.SetParent(hit.collider.transform, false);
                        transform.parent.GetComponent<HexScript>().HandlePlayerEnter(this);
                        _renderer.sortingOrder = hit.collider.GetComponent<SpriteRenderer>().sortingOrder+1;
                        TimeManager.GetTimeManagerInstance().PassTime(_timeTakenToMove * objectOnHex.MovementMultiplier * hex.MovementMultiplier);
                        onPlayerMove?.Invoke();
                        _stepCounter++;
                    }
                    else if (objectOnHex == null)
                    {
                        _renderer.sortingOrder = hit.collider.GetComponent<SpriteRenderer>().sortingOrder+1;
                        transform.SetParent(hit.collider.transform, false);
                        transform.parent.GetComponent<HexScript>()?.HandlePlayerEnter(this);
                        TimeManager.GetTimeManagerInstance().PassTime(_timeTakenToMove * hex.MovementMultiplier);
                        onPlayerMove?.Invoke();
                        _stepCounter++;
                    }
                }
            }
        }
    }

    public void ToggleInventory(InputAction.CallbackContext value)                   //przełączanie ekwipunku
    {
        if (value.started)
        {
            _inventoryHandle.SetActive(!_inventoryHandle.activeSelf);
            SFXManager.GetInstance().Play("EQ");
        }
    }
    public void ToggleCrafting(InputAction.CallbackContext value) //przełączanie ekwipunku
    {
        if (value.started)
        {
            _craftingHandle.SetActive(!_craftingHandle.activeSelf);
            SFXManager.GetInstance().Play("EQ");
        }
    }
    
    public void ToggleBuilding(InputAction.CallbackContext value) //przełączanie ekwipunku
    {
        if (value.started)
        {
            _buildingHandle.SetActive(!_buildingHandle.activeSelf);
            SFXManager.GetInstance().Play("EQ");
        }
    }

    public void Tell(string msg)
    {
        var child = Instantiate(_narratorBlockPrefab, _worldSpaceCanvas.transform);
        child.GetComponent<RectTransform>().DOLocalMove(new Vector3(0,1), 5f);
        var img = child.GetComponent<Image>();
        img.color = new Color(img.color.r, img.color.g, img.color.r, 0);
        var txt = child.GetComponentInChildren<TMP_Text>();
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.r, 0);
        img.DOColor(new Color(img.color.r, img.color.g, img.color.r, 1), 2.5f)
           .OnComplete(() => img.DOColor(new Color(img.color.r, img.color.g, img.color.r, 0), 2.5f)
                                .OnComplete(() =>Destroy(img.gameObject)));
        txt.text = msg;
        txt.DOColor(new Color(txt.color.r, txt.color.g, txt.color.r, 1), 2.5f)
           .OnComplete(() => txt.DOColor(new Color(txt.color.r, txt.color.g, txt.color.r, 0), 2.5f)
                                .OnComplete(() =>Destroy(txt.gameObject)));
    }

    public void LaunchDebugEvent(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            EventManager.GetInstance().LaunchEvent("DebugEvent");
        }
    }

    public object CaptureState()
    {
        return new PlayerSaveData
        {
            HexName = transform.parent.name,
            Stats = stats,
            StepCount = _stepCounter,
            HeroDef = _heroDef
        };
    }

    public void   RestoreState(object state)
    {
        var data = (PlayerSaveData) state;
        transform.SetParent(GameObject.Find(data.HexName).transform, false);
        stats        = data.Stats;
        _stepCounter = data.StepCount;
        _heroDef     = data.HeroDef;
        LoadPlayer();
    }
    [Serializable]
    public struct PlayerSaveData
    {
        public string      HexName;
        public int         StepCount;
        public PlayerStats Stats;
        public string  HeroDef;
    }

    public enum BodyPart
    {
        HEAD, LEFT_ARM, RIGHT_ARM, TORSO, LEFT_LEG, RIGHT_LEG
    }

    private void Update()
    {
        _actionDelay = Mathf.Clamp(_actionDelay - Time.deltaTime, -1, 1);
    }
}