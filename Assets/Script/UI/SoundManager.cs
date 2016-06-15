using UnityEngine;
using System.Collections;

public class SoundManager : Singleton<SoundManager> {

    public AudioClip[] ListBGM;
    private AudioSource _BGMAudio;
    public AudioClip[] ListEffect;
    private AudioSource _EffectAudio;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        ListBGM = new AudioClip[2]
        {
            Resources.Load("Sounds/BG_01", typeof(AudioClip)) as AudioClip,
            Resources.Load("Sounds/BG_02", typeof(AudioClip)) as AudioClip
        };

        ListEffect = new AudioClip[5]
        {
            Resources.Load("Sounds/SD_01",   typeof(AudioClip)) as AudioClip,
            Resources.Load("Sounds/SD_02",   typeof(AudioClip)) as AudioClip,
            Resources.Load("Sounds/SD_04",   typeof(AudioClip)) as AudioClip,
            Resources.Load("Sounds/SD_05",   typeof(AudioClip)) as AudioClip,
            Resources.Load("Sounds/SD_06",   typeof(AudioClip)) as AudioClip
        };

        _BGMAudio = this.gameObject.AddComponent<AudioSource>();
        _EffectAudio = this.gameObject.AddComponent<AudioSource>();

        _BGMAudio.loop = true;
        PlayBGM(1);
    }

    public void PlayBGM(int num)
    {
        switch (num)
        {
            case 1:
                _BGMAudio.clip = ListBGM[0];
                _BGMAudio.volume = 0.7f;
                break;
            case 2:
                _BGMAudio.clip = ListBGM[1];
                _BGMAudio.volume = 1.0f;
                break;
        }
        _BGMAudio.Play();
        _BGMAudio.loop = true;
    }

    public void PlayEffect(string num)
    {
        switch (num)
        {
            case "Touch":
                _EffectAudio.clip = ListEffect[0];
                _EffectAudio.volume = 1.0f;
                break;
            case "Item":
                _EffectAudio.clip = ListEffect[1];
                _EffectAudio.volume = 1.0f;
                break;
            case "Heal":
                _EffectAudio.clip = ListEffect[2];
                _EffectAudio.volume = 1.0f;
                break;
            case "RaisingSword":
                _EffectAudio.clip = ListEffect[3];
                _EffectAudio.volume = 1.0f;
                break;
            case "Brandish":
                _EffectAudio.clip = ListEffect[4];
                _EffectAudio.volume = 1.0f;
                break;
        }
        _EffectAudio.Play();
    }
}
