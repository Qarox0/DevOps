using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class Tooltiper : MonoBehaviour
{
    [SerializeField] private float    _durationOfShow = 1f;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _description;
    
    private                  Color _colorBase;
    private                  Image _renderer;
    public void ShowTooltip(Item item)
    {
        gameObject.SetActive(true);
        var color = _colorBase;
        color.a           = 1;
        
        _title.text       = item._name;
        _description.text = item._decription;
        _description.DOColor(color, _durationOfShow);
        _title.DOColor(color, _durationOfShow);
        _renderer.DOColor(color,_durationOfShow);
    }
    public void HideTooltip()
    {
        var color = _colorBase;
        color.a           = 0;
        _title.text       = "";
        _description.text = "";
        _description.DOColor(color, _durationOfShow);
        _title.DOColor(color, _durationOfShow);
        _renderer.DOColor(color, _durationOfShow);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _renderer  = GetComponent<Image>();
        _colorBase = _renderer.color;
        var color = _colorBase;
        color.a            = 0;
        _renderer.color    = color;
        _title.color       = color;
        _description.color = color;
    }

}
