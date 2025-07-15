using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HidePackUI : MonoBehaviour
{
    [SerializeField] HeroCollide Hero;
    HeroBehavior HeroBehavior;
    private Image imageComponent;
    protected Animator mAnimator = null;
    // Start is called before the first frame update
    void Start()
    {
        Hero = GameObject.Find("Hero").GetComponent<HeroCollide>();
        HeroBehavior = GameObject.Find("Hero").GetComponent<HeroBehavior>();
        imageComponent = GetComponent<Image>();
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Hero.getHidePack) { mAnimator.SetTrigger("GetHidePack"); }

        if (HeroBehavior.IsHidingOnCooldown) { mAnimator.SetBool("IsCooling", true); }
        else { mAnimator.SetBool("IsCooling", false); }

        if (HeroBehavior.IsHiding) { mAnimator.SetBool("IsHiding", true); }
        else { mAnimator.SetBool("IsHiding", false); }
    }
}