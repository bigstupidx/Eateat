using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    private static GameObject container;
    public float                _soundScale = 1;

    public AudioClip[] _arrEffectClip;
    public AudioClip[] _arrBgmClip;
    private Dictionary<string, AudioClip> _dicEffectClip;
    private Dictionary<string, AudioClip> _dicBgmClip;

    public static SoundManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        InitSoundManager();
    }

    void InitSoundManager()
    {
        _dicEffectClip = new Dictionary<string, AudioClip>();
        foreach (AudioClip audioClip in _arrEffectClip)
        {
            _dicEffectClip.Add(audioClip.name, audioClip);
        }

        _dicBgmClip = new Dictionary<string, AudioClip>();
        foreach (AudioClip audioClip in _arrBgmClip)
        {
            _dicBgmClip.Add(audioClip.name, audioClip);
        }
    }

    public void PlayEffectSound(string clipName)
    {
        AudioSource.PlayClipAtPoint(_dicEffectClip[clipName], Vector3.zero, _soundScale);
    }

    public void PlayBgmSound(string clipName)
    {
        AudioSource.PlayClipAtPoint(_dicBgmClip[clipName], Vector3.zero);
    }
}