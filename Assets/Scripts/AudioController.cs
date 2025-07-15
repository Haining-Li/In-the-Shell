using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
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

    private void Start()
    {
        
    }

    public void PlaySfx(AudioClip clip)
    {
        Sfx.PlayOneShot(clip);
    }
}
