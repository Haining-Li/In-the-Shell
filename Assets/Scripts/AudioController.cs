using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    [SerializeField] AudioSource Sfx;

    public AudioClip BloodPack;
    public AudioClip Enemy1Death;
    public AudioClip Enemy2Death;
    public AudioClip Enemy3Death;
    public AudioClip HeroDeath;
    public AudioClip HeroShoot;
    public AudioClip Dash;
    public AudioClip Hide;
    public AudioClip TimeSlow;
    public AudioClip Item;
    public AudioClip HeroHurt;

    void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        LoadVolumeSettings();
    }

    private void Start()
    {

    }

    public void PlaySfx(AudioClip clip)
    {
        Sfx.PlayOneShot(clip);
    }

    public void SetSFXVolume(float volume)
    {
        Sfx.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void LoadVolumeSettings()
    {
        float volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        Sfx.volume = volume;
    }

    // Getter and Setter for Sfx AudioSource
    public AudioSource SfxSource
    {
        get { return Sfx; }
        set { Sfx = value; }
    }
}
