using  UnityEngine;
[CreateAssetMenu(fileName = "Recipie", menuName = "Scriptable/New Recipe")]
public class RecipeObject : ScriptableObject
{
    [SerializeField] public int        _amount1;
    [SerializeField] public GameObject _itemPrefab1;
    [Space]
    [SerializeField] public int _amount2;
    [SerializeField] public GameObject _itemPrefab2;
    [Space]
    [SerializeField] public int        _amount3;
    [SerializeField] public GameObject _itemPrefab3;
    [Space]
    [SerializeField] public int        _amount4;
    [SerializeField] public GameObject _itemPrefab4;
    [Space]
    [SerializeField] public int        _amount5;
    [SerializeField] public GameObject _itemPrefab5;
    [Space] 
    [SerializeField] public int        _outputAmount;
    [SerializeField] public GameObject _outputPrefab;
}

