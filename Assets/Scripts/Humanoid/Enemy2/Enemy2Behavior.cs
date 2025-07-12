using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Behavior : HumanoidBehavior
{
    // Start is called before the first frame update
    public int mBulletNum = 8;
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
            float angle = 360 / mBulletNum;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 origin = mTowards;
            for (int i = 0; i < mBulletNum; i++)
            {
                base.Shoot();
                mTowards = rotation * mTowards;
            }
            mTowards = origin;
            mAnimator.SetTrigger("Shoot");
        }

    }

    public void ShootP()
    {
        if (Time.time - mShootTimer > mShootRate)
        {
            mShootTimer = Time.time;
            float angle = 60 / 5;
            Quaternion rotationP = Quaternion.Euler(0, 0, angle);
            Quaternion rotationN = Quaternion.Euler(0, 0, -angle);
            Vector3 origin = mTowards;
            mTowards = rotationP * mTowards;
            mTowards = rotationP * mTowards;
            for (int i = 0; i < 5; i++)
            {
                base.Shoot();
                mTowards = rotationN * mTowards;
            }
            mTowards = origin;
            mAnimator.SetTrigger("Shoot");
        }
    }
}
