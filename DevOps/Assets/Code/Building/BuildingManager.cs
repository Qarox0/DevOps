using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private List<BuildingRecipeObject> _buildings;
    [SerializeField] private Button                     _buildButton;
    [SerializeField] private Transform                  _buildingList;
    [SerializeField] private GameObject                 _buildingFieldPrefab;

    private GameObject _player;
    private GameObject _selectedBuildingPrefab;

    private static BuildingManager _instance;

    public void SetSelection(GameObject selected)
    {
        if (_selectedBuildingPrefab != null)
        {
            _selectedBuildingPrefab.GetComponent<Image>().color = Color.white;
        }

        _selectedBuildingPrefab = selected;
        _selectedBuildingPrefab.GetComponent<Image>().color = Color.gray;
    }

    public static BuildingManager GetBuildingManagerInstance()
    {
        if (_instance == null) _instance = FindObjectOfType<BuildingManager>();
        return _instance;
    }
    // Start is called before the first frame update
    private void Start()
    {
        _player = FindObjectOfType<Player>().gameObject;
        if (_player == null)
        {
            Debug.LogError("PlayerNotFoundException");
        }
        _buildButton.onClick.AddListener(Build);
        foreach (var building in _buildings)
        {
            var child = Instantiate(_buildingFieldPrefab, _buildingList);
            child.GetComponent<SelectableField>().BuildRecipe        = building;
            child.transform.GetChild(0).GetComponent<Image>().sprite = building.BuildMenuImage;
        }
        
    }

    public void Build()
    {
        var standingHex = _player.GetComponentInParent<HexScript>();
        var fieldSelected      = _selectedBuildingPrefab.GetComponent<SelectableField>();
        if (standingHex.IsHexEmpty())
        {
            if (Inventory.GetInventoryInstance()
                         .CheckForBuildingRecipe(fieldSelected.BuildRecipe))
            {
                foreach (var itemRequired in fieldSelected.BuildRecipe.ItemsNeeded)
                {
                    Inventory.GetInventoryInstance()
                             .SubstractFromInventory(itemRequired.ItemNeeded.GetComponent<Item>(), itemRequired.Amount);
                }

                var builded =Instantiate(fieldSelected.BuildRecipe.Output, standingHex.transform);
                standingHex.SetObjectOnField(builded);
            }
        }
    }
}
