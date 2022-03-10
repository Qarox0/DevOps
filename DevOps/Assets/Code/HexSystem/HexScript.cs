using System;
using System.Collections;
using System.Collections.Generic;
using Code.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
public class HexScript : MonoBehaviour, ISaveable
{
    [Header("Grid Fields")] [Tooltip("Field for object attached on this hexagon")] [SerializeField] 
    #if UNITY_EDITOR
    [RequireInterface(typeof(IHexable))]
    #endif
    private Object _objectOnField;    //Obiekt interaktywny na hexie
    [Tooltip("radius of circle drawn to find nearby hexes")]
    [SerializeField] private float _radiusOfNearHexCheck = 5f;
    [SerializeField] public int MovementMultiplier = 1;
    [Space]
    [Header("debug")]
    [SerializeField] private bool             _isDrawingGizmos = true;
    private                  List<GameObject> _surroundingHexes;

    private void Start()
    {
        _surroundingHexes = new List<GameObject>();
        RaycastHit2D[] raycastHits2D =
            Physics2D.CircleCastAll(transform.position, _radiusOfNearHexCheck, Vector2.zero);
        foreach (var hit in raycastHits2D)
        {
            if (hit.collider.tag == "Hex")
            {
                _surroundingHexes.Add(hit.collider.gameObject);
            }
        }
        Debug.Log("Executed");
    }

    public bool IsHexEmpty()
    {
        if (_objectOnField == null)
        {
            return true;
        }

        return false;
    }
    #if UNITY_EDITOR
    public void OnDrawGizmosSelected()      //rysuje gizmos jeśli jest zaznaczone
    {
        if (_isDrawingGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, _radiusOfNearHexCheck);
        }
    }
    #endif
    public bool IsAdjecent(GameObject hexToCheck)    //Czy hex jest sąsiadujący
    {
        foreach (var hex in _surroundingHexes)
        {
            if (hex.GetInstanceID() == hexToCheck.GetInstanceID())
            {
                return true;
            }
        }

        return false;
    }

    public void SetObjectOnField(GameObject objectPrefab)
    {
        _objectOnField = objectPrefab;
    }

    public GameObject GetFishingSpot()
    {
        foreach (var hex in _surroundingHexes)
        {
            if (hex.GetComponentInChildren<FishableHex>() != null)
            {
                return hex;
            }
            
        }
        return null;
    }

    public void HandlePlayerEnter(Player player) //Metoda odpowiadająca za wejscie gracza na pole
    {
        if (_objectOnField != null)             //Jeśli obiekt nie jest pusty
        {
            
            var hexConversion = _objectOnField as IHexable; //Konwertuj na Hexable
            if (hexConversion != null &&                    //czy konwersja się udała
                hexConversion.IsLaunchingOnEnter )          //i sprawdź czy reaguje na starcie
            {
                hexConversion.Interaction(player); //Niech zareaguje
            }
        }
    }


    public void HandlePlayerInteraction(Player player)  //Metoda decyzyjnej interakcji z graczem
    {
        if (_objectOnField != null) //Jeśli obiekt nie jest pusty
        {
            var hexConversion = _objectOnField as IHexable; //Konwertuj na Hexable
            if (hexConversion != null)                      //Sprawdź czy konwersja się udała
            {
                hexConversion.Interaction(player); //Reaguj
            }

            var hexGameObject = _objectOnField as GameObject;
            if (hexGameObject != null)
            {
                hexGameObject.GetComponent<IHexable>().Interaction(player);
            }
        }
    }

    public object CaptureState()
    {
        string       hexObjectPrefabName = "";
        int          UseCount            = 0;
        int          GrowTime            = 0;
        CatchEnum    Catched             = default;
        bool         IsCatched           = false;
        RequiredItem BaitInSlot          = default;
        RequiredItem TrapInSlot          = default;
        int          FuelBurningTime     = 0;
        string       FuelPrefab          = "";
        int          TimePassed          = 0;
        string       Recipe              = "";
        RequiredItem Output              = default;
        bool         IsCooking           = false;
        int          FuelAmount          = 0;
        if (_objectOnField != null) //Jeśli obiekt nie jest pusty
        {
            var hexConversion = _objectOnField as IHexable; //Konwertuj na Hexable
            if (hexConversion != null)                      //Sprawdź czy konwersja się udała
            {
                

                hexObjectPrefabName = hexConversion.PrefabName;
                UseCount            = hexConversion.UseCount;
                GrowTime            = hexConversion.GrowTime;
                Catched             = hexConversion.Catched;
                IsCatched           = hexConversion.IsCatched;
                BaitInSlot          = hexConversion.BaitInSlot;
                TrapInSlot          = hexConversion.TrapInSlot;
                FuelBurningTime     = hexConversion.FuelBurningTime;
                FuelPrefab          = hexConversion.FuelPrefab;
                TimePassed          = hexConversion.TimePassed;
                Recipe              = hexConversion.Recipe;
                Output              = hexConversion.Output;
                IsCooking           = hexConversion.IsCooking;
                FuelAmount          = hexConversion.FuelAmount;
                Debug.Log(hexObjectPrefabName);
            }
            var hexGameObject = _objectOnField as GameObject;
            if (hexGameObject != null)
            {
                hexObjectPrefabName = hexGameObject.GetComponent<IHexable>().PrefabName;
                UseCount         = hexGameObject.GetComponent<IHexable>().UseCount;
                GrowTime         = hexGameObject.GetComponent<IHexable>().GrowTime;
                Catched          = hexGameObject.GetComponent<IHexable>().Catched;
                IsCatched        = hexGameObject.GetComponent<IHexable>().IsCatched;
                BaitInSlot       = hexGameObject.GetComponent<IHexable>().BaitInSlot;
                TrapInSlot       = hexGameObject.GetComponent<IHexable>().TrapInSlot;
                FuelBurningTime  = hexGameObject.GetComponent<IHexable>().FuelBurningTime;
                FuelPrefab       = hexGameObject.GetComponent<IHexable>().FuelPrefab;
                TimePassed       = hexGameObject.GetComponent<IHexable>().TimePassed;
                Recipe           = hexGameObject.GetComponent<IHexable>().Recipe;
                Output           = hexGameObject.GetComponent<IHexable>().Output;
                IsCooking        = hexGameObject.GetComponent<IHexable>().IsCooking;
                FuelAmount       = hexGameObject.GetComponent<IHexable>().FuelAmount;
                Debug.Log(hexObjectPrefabName);
            }
        }
        
        return new HexSaveData
        {
            RadiusOfNearHexCheck    = _radiusOfNearHexCheck,
            MovementMultiplier      = MovementMultiplier,
            ObjectOnFieldPrefabName = hexObjectPrefabName,
            UseCount                = UseCount,
            GrowTime                = GrowTime,
            Catched                 = Catched,
            IsCatched               = IsCatched,
            BaitInSlot              = BaitInSlot,
            TrapInSlot              = TrapInSlot,
            FuelBurningTime         = FuelBurningTime,
            FuelPrefab              = FuelPrefab,
            TimePassed              = TimePassed,
            Recipe                  = Recipe,
            Output                  = Output,
            IsCooking               = IsCooking,
            FuelAmount              = FuelAmount
        };
    }
    

    public void   RestoreState(object state)
    {
        if (gameObject.transform.childCount > 0)
        {
            if(gameObject.transform.GetChild(0).GetComponent<IHexable>() != null)
                Destroy(gameObject.transform.GetChild(0).gameObject);
        }
        var data = (HexSaveData) state;
        var hexObj = Resources.Load<GameObject>(GlobalConsts.PathToHexObjects + data.ObjectOnFieldPrefabName);
        if (hexObj != null)
        {
            var child = Instantiate(hexObj,
                gameObject.transform,
            false);
            SetObjectOnField(child);
            if (_objectOnField != null)
            {
                var hexConversion = _objectOnField as IHexable;
                if (hexConversion != null) //Sprawdź czy konwersja się udała
                {
                    hexConversion.PrefabName      = data.ObjectOnFieldPrefabName;
                    hexConversion.UseCount        = data.UseCount;
                    hexConversion.GrowTime        = data.GrowTime;
                    hexConversion.Catched         = data.Catched;
                    hexConversion.IsCatched       = data.IsCatched;
                    hexConversion.BaitInSlot      = data.BaitInSlot;
                    hexConversion.TrapInSlot      = data.TrapInSlot;
                    hexConversion.FuelBurningTime = data.FuelBurningTime;
                    hexConversion.FuelPrefab      = data.FuelPrefab;
                    hexConversion.TimePassed      = data.TimePassed;
                    hexConversion.Recipe          = data.Recipe;
                    hexConversion.Output          = data.Output;
                    hexConversion.IsCooking       = data.IsCooking;
                    hexConversion.FuelAmount      = data.FuelAmount;
                    Debug.Log("Restored");

                }
                var hexGameObject = _objectOnField as GameObject;
                if (hexGameObject != null)
                {
                    hexGameObject.GetComponent<IHexable>().PrefabName      = data.ObjectOnFieldPrefabName;
                    hexGameObject.GetComponent<IHexable>().UseCount        = data.UseCount;
                    hexGameObject.GetComponent<IHexable>().GrowTime        = data.GrowTime;
                    hexGameObject.GetComponent<IHexable>().Catched         = data.Catched;
                    hexGameObject.GetComponent<IHexable>().IsCatched       = data.IsCatched;
                    hexGameObject.GetComponent<IHexable>().BaitInSlot      = data.BaitInSlot;
                    hexGameObject.GetComponent<IHexable>().TrapInSlot      = data.TrapInSlot;
                    hexGameObject.GetComponent<IHexable>().FuelBurningTime = data.FuelBurningTime;
                    hexGameObject.GetComponent<IHexable>().FuelPrefab      = data.FuelPrefab;
                    hexGameObject.GetComponent<IHexable>().TimePassed      = data.TimePassed;
                    hexGameObject.GetComponent<IHexable>().Recipe          = data.Recipe;
                    hexGameObject.GetComponent<IHexable>().Output          = data.Output;
                    hexGameObject.GetComponent<IHexable>().IsCooking       = data.IsCooking;
                    hexGameObject.GetComponent<IHexable>().FuelAmount      = data.FuelAmount;
                    Debug.Log("Restored");

                }
            }
        }
    }
    [Serializable]
    private struct HexSaveData
    {
        public float        RadiusOfNearHexCheck;
        public int          MovementMultiplier;
        public string       ObjectOnFieldPrefabName;
        public int          UseCount;
        public int          GrowTime;
        public CatchEnum    Catched;
        public bool         IsCatched;
        public RequiredItem BaitInSlot;
        public RequiredItem TrapInSlot;
        public int          FuelBurningTime;
        public string       FuelPrefab;
        public int          TimePassed;
        public string       Recipe;
        public RequiredItem Output;
        public bool         IsCooking;
        public int          FuelAmount;
    }
}
