using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Behavior : HumanoidBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
        Debug.Assert(mAnimator != null);
    }

    // Update is called once per frame

    public override void Sleep()
    {
        
    }

    public override void Activate()
    {
        mAnimator.SetTrigger("Activate");
    }

    public override void Idle()
    {
        base.Idle();
        mAnimator.SetFloat("Speed", 0f);
    }

    public override void Move()
    {
        base.Move();
        mAnimator.SetFloat("Speed", 1f);
    }

    public override void Shoot()
    {
        if (Time.time - mShootTimer > mShootRate)
        {
            mShootTimer = Time.time;
            base.Shoot();
            mAnimator.SetTrigger("Shoot");
        }
        
    }
}
