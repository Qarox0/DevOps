using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private List<AudioTrack> _audioClips;
    // Start is called before the first frame update
    private static SFXManager  _instance;
    private        AudioSource _source;

    public static SFXManager GetInstance()
    {
        if (_instance == null) _instance = FindObjectOfType<SFXManager>();
        return _instance;
    }

    public void Play(string name)
    {
        foreach (var track in _audioClips)
        {
            if (track.Name == name)
            {
                _source.PlayOneShot(track.Clip);
            }
        }
    }

    void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[Serializable]
struct AudioTrack
{
    public AudioClip Clip;
    public string    Name;
}
