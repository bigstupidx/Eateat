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
        //    if (instance == null)
        //    {
        //        instance = (SoundManager)FindObjectOfType(typeof(SoundManager));
        //        if (instance == null)
        //        {
        //            SoundManager prefab = (SoundManager)Resources.FindObjectsOfTypeAll(typeof(SoundManager));
        //            if (prefab != null)
        //            {
        //                instance = (SoundManager)Instantiate(prefab);
        //            }
        //            else
        //            {
        //                instance = (new GameObject()).AddComponent<SoundManager>();
        //            }
        //            instance.gameObject.name = "(singleton) " + typeof(SoundManager).ToString();
        //        }
        //        DontDestroyOnLoad(instance.gameObject);
        //    }
        //    return instance;
        }
    }
    //get
    //{
    //    if (instance == null)
    //    {
    //        //container = new GameObject();
    //        //container.name = "SoundManager";
    //        //instance = container.AddComponent<SoundManager>();
    //        //instance = GameObject.FindObjectOfType(typeof(SoundManager)) as SoundManager;
    //        //instance = new SoundManager();

    //        instance = instance.gameObject.GetComponent<SoundManager>();

    //        instance.InitSoundManager();
    //        DontDestroyOnLoad(instance.gameObject);
    //    }
    //    return instance;
    //}
    //}

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        InitSoundManager();
    }

    //Initializes the game for each level.
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

    void Update()
    {

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