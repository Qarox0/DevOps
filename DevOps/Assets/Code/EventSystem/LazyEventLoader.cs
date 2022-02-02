using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LazyEventLoader : MonoBehaviour
{
    private Dictionary<string, EventObject> _events;
    // Start is called before the first frame update
    void Start()
    {
        _events = new Dictionary<string, EventObject>();
    }

    public EventObject GetEvent(string name)
    {
        EventObject _event = null;
        if (_events.ContainsKey(name))
        {
            _event = _events.First(x => x.Key == name).Value;
        }
        else
        {
            _event = Resources.Load($"ScriptableObjects/Events/{name}") as EventObject;
            _events.Add(name, _event);
        }
        return _event;
    }
}
