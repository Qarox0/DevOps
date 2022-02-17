using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    [SerializeField] private Transform _trapPanelTransform;
    [SerializeField] private Transform _baitSlot;
    [SerializeField] private Transform _trapSlot;

    private        bool        _isOpen = false;
    private        TrapHex     _actualTrap;
    private static TrapManager _instance;
    

    public static TrapManager GetInstance()
    {
        if (_instance == null) _instance = FindObjectOfType<TrapManager>();
        return _instance;
    }

    private void Start()
    {
        FindObjectOfType<Player>().onPlayerMove += PlayerMoved;
    }

    private void PlayerMoved()
    {
        if(_isOpen) Close();
        _trapPanelTransform.gameObject.SetActive(false);
    }

    private void Close()
    {
        if(_baitSlot.childCount > 0 || _trapSlot.childCount > 0)
            startCatching();
    }

    public void ToggleTrapPanel(TrapHex trap)
    {
        _trapPanelTransform.gameObject.SetActive(!_trapPanelTransform.gameObject.activeSelf);
        init(trap);
    }

    private void init(TrapHex trap)
    {
        if (!trap.TrapInSlot.Equals(default(RequiredItem)))
        {
            var titem = Instantiate(trap.TrapInSlot.ItemNeeded, _trapSlot);
            titem.GetComponent<Item>().Quantity = trap.TrapInSlot.Amount;
        }

        if (!trap.BaitInSlot.Equals(default(RequiredItem)))
        {
            var bitem = Instantiate(trap.BaitInSlot.ItemNeeded, _baitSlot);
            bitem.GetComponent<Item>().Quantity = trap.BaitInSlot.Amount;
        }

        _isOpen                             = true;
        _actualTrap                         = trap;
    }

    private void startCatching()
    {
        TimeManager.GetTimeManagerInstance().onTimePasses += _actualTrap.TryCatch;
        if (_baitSlot.childCount > 0)
        {
            var child = _baitSlot.GetComponentInChildren<Item>();
            _actualTrap.BaitInSlot.ItemNeeded = child.gameObject;
            _actualTrap.BaitInSlot.Amount     = child.Quantity;
        }
        if (_trapSlot.childCount > 0)
        {
            var child = _trapSlot.GetComponentInChildren<Item>();
            _actualTrap.TrapInSlot.ItemNeeded = child.gameObject;
            _actualTrap.TrapInSlot.Amount     = child.Quantity;
        }
    }
}
