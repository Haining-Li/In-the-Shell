using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BossBehavior : HumanoidBehavior
{
    // Start is called before the first frame update
    Animator mLowerBody = null;
    void Start()
    {
        Init();
        mLowerBody = GetComponentsInChildren<Animator>()[1];
        Debug.Assert(mLowerBody != null, "No Lowerbody Animator");
        Debug.Assert(mAnimator != null);
    }

    // Update is called once per frame
    public override void Idle()
    {
        base.Idle();
        mAnimator.SetFloat("Speed", 0f);
        mLowerBody.SetFloat("Speed", 0f);
    }

    public override void Move()
    {
        base.Move();
        mAnimator.SetFloat("Speed", 1f);
        mLowerBody.SetFloat("Speed", 1f);
    }

    public override void Shoot()
    {
        if (Time.time - mShootTimer > mShootRate)
        {
            mShootTimer = Time.time;
            base.Shoot();
            mAnimator.SetTrigger("Shoot");
            mAnimator.SetInteger("ShootMode", 1);
        }
        
    }
}
