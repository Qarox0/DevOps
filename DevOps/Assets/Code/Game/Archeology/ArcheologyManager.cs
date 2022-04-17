using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ArcheologyManager : MonoBehaviour
{
    [SerializeField] private GameObject           _archeologyHolder;
    [SerializeField] private ArcheologyDataObject _data;
    [SerializeField] private GameObject           _blocker;
    
    private int         _findCount = 0;
    private List<int>   _slots;
    public  bool        IsReadyForNext { get; private set; }
    private MatchHolder _item1;

    private static ArcheologyManager _instance;

    public static ArcheologyManager GetInstance()
    {
        if (_instance == null) _instance = FindObjectOfType<ArcheologyManager>();
        return _instance;
    }

    public void OpenArcheology(ArcheologyDataObject data)
    {
        _archeologyHolder.SetActive(true);
        _blocker.SetActive(true);
        FindObjectOfType<PlayerInput>().DeactivateInput();
        _data                                         = data;
        GenerateArcheology();
    }
    

    // Start is called before the first frame update
    void Start()
    {
        IsReadyForNext = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(MatchHolder holder)
    {
        if (_item1 == null)
        {
            _item1 = holder;
        }
        else
        {
            if (_item1.SettedImage.sprite == holder.SettedImage.sprite)
            {

                _findCount++;
                if (_findCount >= _archeologyHolder.transform.childCount/2)
                {
                    foreach (var reward in _data.Rewards)
                    {
                        Inventory.GetInventoryInstance().AddItemToInventory(reward);
                    }
                    _archeologyHolder.SetActive(false);       
                    _blocker.SetActive(false);
                    FindObjectOfType<PlayerInput>().ActivateInput();
                    for (int i = 0; i < _archeologyHolder.transform.childCount; i++)
                    {
                        _archeologyHolder.transform.GetChild(i).GetComponent<Image>().sprite = null;
                    }
                }
            }
            else
            {

                _item1.FlipAndHide();
                holder.FlipAndHide();
            }

            _item1 = null;
        }
        
    }

    private void GenerateArcheology()
    {
        var childCount = _archeologyHolder.transform.childCount;
        _slots = new List<int>();
        for (int j = 0; j < childCount; j++)
        {
            _slots.Add(j);
        }
        _slots = _slots.OrderBy(a => Random.Range(0, childCount -1)).ToList();
        for (int i = 0; i < childCount/2; i++)
        {
            int first  = _slots[i];
            int second = _slots[i +childCount /2];
            

            
            
            var randomImage = _data.ImagesToGenerateList[Random.Range(0, _data.ImagesToGenerateList.Count)];
            _archeologyHolder.transform.GetChild(first).GetComponent<MatchHolder>().SettedImage = randomImage;
            _archeologyHolder.transform.GetChild(second).GetComponent<MatchHolder>().SettedImage = randomImage;
        }
    }
}
