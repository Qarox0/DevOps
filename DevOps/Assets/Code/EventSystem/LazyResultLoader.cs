using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LazyResultLoader : MonoBehaviour
{
    private Dictionary<string, Transform> _results;

    [SerializeField] private Transform garbageCollector;
    // Start is called before the first frame update
    void Start()
    {
        _results = new Dictionary<string, Transform>();
    }

    public IEventResult GetResult(string name)
    {
        IEventResult result = null;
        Transform   scriptHolder;
        if (_results.ContainsKey(name))
        {
            result = _results.First(x => x.Key == name).Value.GetComponent<IEventResult>();
        }
        else
        {
            scriptHolder = garbageCollector.Find(name);
            _results.Add(name, scriptHolder);
            result = scriptHolder.GetComponent<IEventResult>();
        }
        return result;
    }
}
