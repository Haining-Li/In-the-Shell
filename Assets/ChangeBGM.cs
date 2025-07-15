using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : MonoBehaviour
{
    // Start is called before the first frame update
    BossAI Boss;
    bool flag;
    AudioSource mAudioSource;

    public AudioClip bgm1;
    public AudioClip bgm2;
    void Start()
    {
        flag = true;
        Boss = GameObject.Find("Boss").GetComponent<BossAI>();
        mAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flag && Boss.isChangingMode)
        {
            mAudioSource.clip = bgm2;
            mAudioSource.Play();
        }
        
    }
}
