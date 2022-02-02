using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LazyAnswerLoader : MonoBehaviour
{
    private Dictionary<string, AnswerObject> _answers;
    // Start is called before the first frame update
    void Start()
    {
        _answers = new Dictionary<string, AnswerObject>();
    }

    public AnswerObject GetAnswer(string name)
    {
        AnswerObject answer = null;
        if (_answers.ContainsKey(name))
        {
            answer = _answers.First(x => x.Key == name).Value;
        }
        else
        {
            answer = Resources.Load($"ScriptableObjects/Answers/{name}") as AnswerObject;
            _answers.Add(name, answer);
        }
        return answer;
    }
}
