using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableField : MonoBehaviour, IPointerClickHandler
{
    public BuildingRecipeObject BuildRecipe;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BuildingManager.GetBuildingManagerInstance().SetSelection(gameObject);
    }
}
