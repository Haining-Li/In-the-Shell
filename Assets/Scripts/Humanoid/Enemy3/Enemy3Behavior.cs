using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Behavior : HumanoidBehavior
{
    // Start is called before the first frame update
    public float mSectorRange = 5f;
    public int mBulletNum = 5;
    void Start()
    {
        Init();
        Debug.Assert(mAnimator != null);
    }

    // Update is called once per frame

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

            Vector3 origin = mTowards;
            for (int i = 0; i < mBulletNum; i++)
            {
                float angle = Random.Range(-mSectorRange, mSectorRange);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                mTowards = rotation * origin;
                base.Shoot();
            }
            mTowards = origin;
            mAnimator.SetTrigger("Shoot");
        }
    }
}
