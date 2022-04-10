using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class MatchHolder : MonoBehaviour, IPointerClickHandler
{
    public  Image         SettedImage;
    private Image         _actualImage;
    private RectTransform _transform;
    private bool          _isShown = false;

    private void OnEnable()
    {
        QuickFlip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && ArcheologyManager.GetInstance().IsReadyForNext)
        {
            if (!_isShown)
            {
                FlipAndShow();
            }
        }
    }

    private void QuickFlip()
    {
        if (_transform != null && SettedImage != null)
        {
            _transform.DOScale(0, 0.25f).OnComplete(() =>
            {
                _actualImage.sprite = SettedImage.sprite;
                _transform.DOScale(1, 0.25f).OnComplete(() => _transform.DOScale(0, 0.25f).OnComplete(() =>
                {
                    _actualImage.sprite = null;
                    _transform.DOScale(1, 0.25f);
                    _isShown = false;
                }));

            });
        }
    }
    private void FlipAndShow()
    {
        _isShown = true;
        _transform.DOScale(0, 0.25f).OnComplete(() =>
        {
            _actualImage.sprite = SettedImage.sprite;
            _transform.DOScale(1, 0.25f).OnComplete(()=>ArcheologyManager.GetInstance().SetItem(this));
            
        });
    }
    public void FlipAndHide()
    {
        _isShown = false;
        _transform.DOScale(0, 0.25f).OnComplete(() =>
        {
            _actualImage.sprite = null;
            _transform.DOScale(1, 0.25f);
        });
    }
    private void Start()
    {
        _actualImage = GetComponent<Image>();
        _transform   = GetComponent<RectTransform>();
        QuickFlip();
    }
}
