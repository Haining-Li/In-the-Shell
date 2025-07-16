using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Collide : HumanoidCollide
{
    Animator mAnimator;
    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
    }
}
