using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = System.Random;

//Klasa itemu przypisanego do slota
public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("How much item weight")]
    [SerializeField] private float    _weight;                //Waga itemu
    [Tooltip("How many will be in stack")]
    [SerializeField] private int      _maxStackQuantity = 64; //Max liczba w slocie
    [Tooltip("Item name")]
    [SerializeField] public string _name;   //Nazwa
    [Tooltip("Item description")]
    [SerializeField] public string _decription;   //opis
    [Tooltip("Which item type is this item")]
    [SerializeField] private ItemType _type;   //typ itemu
    public                   int   _fuelValue; //wyjątkowe złamanie zasad dobrego kodzenia
    [SerializeField] private int _durability = 1;
    
    [Space]
    [Header("UI")]
    [Tooltip("Display for quantity")]
    [SerializeField] private Text _text;                                   //Pole Textowe do obsługi ilości
    
    [Space]
    [Header("Eating")]
    [SerializeField]                             private bool   _isEdible; //Czy jest jadalny
    [SerializeField]
    private string _edibleParams = "";
    [Tooltip("What is happening on eat")]  [SerializeField]
    #if UNITY_EDITOR
    [RequireInterface(typeof(IEdible))]
    #endif
    private Object _edibleImplementation; //Referencja do implementacji edible
    
    [Space]
    [Header("Trap")]
    [SerializeField] private List<CatchEnum> _catchDictionary; //Lista eventów do łapania
    [SerializeField] private float  _catchMultiplier;          //Mnożnika szansy na złapanie
    [SerializeField] public  string PrefabName;
    

    private       int         _quantity = 0;    //ile jest aktualnie
    private       CanvasGroup _canvasGroup;     //komponent canvas group do draga
    private       bool        _isDragging;      //Czy jest draggowany
    private       Transform   _oldParent;       //Parent do dragga
    private       Transform   _oldParentCopy;   //Parent do dragga
    private       Canvas      _canvas;          //Do Dragga
    private       bool        _isDroppedOnSlot; //Do Dragga
    private       bool        _isOnItem;        //Do Dragga
    private const int         MINCHANCE       = 0;
    private const int         MAXCHANCE       = 100;
    private       Tooltiper   _tooltiper;
    public int Quantity
    {
        get { return _quantity;}
        set
        {
            _quantity   = value;
            _text.text = value.ToString();
        }
    }

    public int GetDurability()
    {
        return _durability;
    }

    public void DamageItem(int amount)
    {
        _durability -= amount;
    }
    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvas      = GetComponentInParent<Canvas>();
        _tooltiper   = Tooltiper.GetInstane();
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

    public CatchEnum GetCatch()
    {
        CatchEnum catchEnum = new CatchEnum();
        foreach (var catching in _catchDictionary)
        {
            Random random = new Random();
            var roll = random.Next(MINCHANCE, MAXCHANCE);
            Debug.Log($"They see me rolling:{roll} - {catching.EventChance}");
            if (roll <= catching.EventChance)
            {
                return catching;
            }
        }

        return catchEnum;
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
        // else if (Keyboard.current.leftShiftKey.isPressed)
        // {
        //     Divider.GetInstance().init(gameObject);
        // }
        transform.SetParent(_oldParent,false);
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        
    }

    #endregion

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && _isEdible)
        {
            var edibleConversion = _edibleImplementation as IEdible; //Konwertuj na Edible
            if (edibleConversion != null)                            
            {
                edibleConversion.Eat(_edibleParams, this);
            }

            var edibleGameObject = _edibleImplementation as GameObject;
            if (edibleGameObject != null)
            {
                edibleGameObject.GetComponent<IEdible>().Eat(_edibleParams, this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tooltiper.ShowTooltip(this);
    }

    public void OnPointerExit(PointerEventData  eventData)
    {
        _tooltiper.HideTooltip();
    }
}

public enum ItemType    //typ itemu
{
    RESOURCE, TOOL, FOOD
}
