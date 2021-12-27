using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

//Klasa itemu przypisanego do slota
public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Tooltip("How much item weight")]
    [SerializeField] private float    _weight;                //Waga itemu
    [Tooltip("How many will be in stack")]
    [SerializeField] private int      _maxStackQuantity = 64; //Max liczba w slocie
    [Tooltip("Item name")]
    [SerializeField] private string _name;   //Nazwa
    [Tooltip("Which item type is this item")]
    [SerializeField] private ItemType _type; //typ itemu
    [Space]
    [Header("UI")]
    [Tooltip("Display for quantity")]
    [SerializeField] private Text     _text;                  //Pole Textowe do obsługi ilości

    private int         _quantity = 0;  //ile jest aktualnie
    private CanvasGroup _canvasGroup;   //komponent canvas group do draga
    private bool        _isDragging;    //Czy jest draggowany
    private Transform   _oldParent;     //Parent do dragga
    private Transform   _oldParentCopy; //Parent do dragga
    private Canvas      _canvas;        //Do Dragga
    private bool        _isDroppedOnSlot;//Do Dragga
    private bool        _isOnItem;      //Do Dragga
    
    public int Quantity
    {
        get { return _quantity;}
        set
        {
            _quantity   = value;
            _text.text = value.ToString();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvas      = GetComponentInParent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDragging)
        {
            this.transform.position = Mouse.current.position.ReadValue();
        }
    }

    #region getters & setters
    public float GetWeight()
    {
        return _weight;
    }
    public string GetName()
    {
        return _name;
    }
    public int GetMaxStackQuantity()
    {
        return _maxStackQuantity;
    }    
    public bool IsDroppedOnSlot
    {
        get { return _isDroppedOnSlot; }
        set { _isDroppedOnSlot = value; }
    }

    public bool IsOnItem
    {
        get { return _isOnItem; }
        set { _isOnItem = value; }
    }
    public void SetParent(Transform parent)
    {
        if (Keyboard.current.altKey.isPressed)
        {
            _oldParentCopy = _oldParent;
        }
        _oldParent = parent;

    }
    #endregion

    #region Dragging

    public void OnBeginDrag(PointerEventData eventData)
    {
        _oldParent                  = transform.parent;
        _isDragging                 = true;
        _isDroppedOnSlot            = false;
        _isOnItem                   = false;
        _canvasGroup.blocksRaycasts = false;
        transform.SetParent(_canvas.transform,true);
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("End");
        _isDragging                 = false;
        _canvasGroup.blocksRaycasts = true;
        if (Keyboard.current.altKey.isPressed)
        {
            if (Quantity > 1 && !_isOnItem && _oldParentCopy != null && _oldParent.GetInstanceID() != _oldParentCopy.GetInstanceID())
            {
                //Debug.Log("Not on item");
                var copy = Instantiate(gameObject, _oldParentCopy);
                copy.GetComponent<Item>().Quantity = Quantity - 1;
                Quantity                           = 1;
            }

        }
        transform.SetParent(_oldParent,false);
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        
    }

    #endregion
    
}

public enum ItemType    //typ itemu
{
    RESOURCE, TOOL
}
