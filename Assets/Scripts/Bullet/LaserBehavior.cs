using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : ProjectileBehavior
{
    void Awake()
    {
        mStatusTimer = Time.time;
        mStatus = ProjectileStatus.Flying;
        mDamage = 10;
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        switch (mStatus)
        {
            case ProjectileStatus.Flying:
                Move();
                break;
            case ProjectileStatus.Crash:
                Hit();
                break;
            case ProjectileStatus.Destroyed:
                Destroy();
                break;
        }
    }

    public override void Move()
    {
        if (Time.time - mStatusTimer < mFlyingDuration)
        {
            base.Move();
        }
        else
        {
            mStatus = ProjectileStatus.Destroyed;
        }
    }

    public override void Hit()
    {
        if (Time.time - mStatusTimer < mHitDuration)
        {
            base.Hit();
        }
        else
        {
            mStatus = ProjectileStatus.Destroyed;
        }

    }

    public override void Destroy()
    {
        base.Destroy();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject e = collision.gameObject;
        
        if (e.layer == 31 && ((Hero && e.CompareTag("Player")) || (Enemy && e.CompareTag("Enemy"))))
        {
            if (mStatus == ProjectileStatus.Flying)
            {
                mStatusTimer = Time.time;
                mStatus = ProjectileStatus.Crash;
                mAnimator.SetTrigger("Destroy");
                if(e.GetComponent<HumanoidStatus>().mHealthPoint > 0)
                {
                    e.GetComponent<HumanoidStatus>().GetHurt(mDamage);
                }
            }
        }
        else if (e.layer != 31)
        {
            if (mStatus == ProjectileStatus.Flying)
            {
                mStatusTimer = Time.time;
                mStatus = ProjectileStatus.Crash;
                mAnimator.SetTrigger("Destroy");
            }
        }
    }
}
