using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPUPackUI : MonoBehaviour
{
    [SerializeField] HeroCollide Hero;
    HeroBehavior HeroBehavior;
    private Image imageComponent;
    protected Animator mAnimator = null;
    // Start is called before the first frame update
    void Start()
    {
        HeroBehavior = GameObject.Find("Hero").GetComponent<HeroBehavior>();
        Hero = GameObject.Find("Hero").GetComponent<HeroCollide>();
        imageComponent = GetComponent<Image>();
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Hero.getCPUPack) { mAnimator.SetTrigger("getCPUPack"); }

        if (HeroBehavior.IsOnCooldown) { mAnimator.SetBool("IsCoolingDown", true); }
        else { mAnimator.SetBool("IsCoolingDown", false); }

        if (HeroBehavior.IsTimeSlowingActive) { mAnimator.SetBool("IsTimeSlowing",true); }
        else { mAnimator.SetBool("IsTimeSlowing", false); }
    }
}