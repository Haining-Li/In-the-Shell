using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBehavior : ProjectileBehavior
{
    public int mCrashCount = 2;
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
        // 提前忽略所有敌人的碰撞（适用于已知敌人列表的情况）
        GameObject[] ignores = null;
        if (!Hero)
            ignores = GameObject.FindGameObjectsWithTag("Player");
        if (!Enemy)
            ignores = GameObject.FindGameObjectsWithTag("Enemy");
        
        Collider2D bulletCollider = GetComponent<Collider2D>();
    
        foreach (var enemy in ignores)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                Physics2D.IgnoreCollision(bulletCollider, enemyCollider);
            }
        }
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject e = collision.gameObject;
        
        if ((Hero && e.layer == LayerMask.NameToLayer("Hero")) || (Enemy && e.layer == LayerMask.NameToLayer("Enemy")))
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
        else if (e.layer == LayerMask.NameToLayer("Default"))
        {
            Debug.Log("Enter" + mCrashCount);
            if (mStatus == ProjectileStatus.Flying)
            {
                mStatusTimer = Time.time;
                if (mCrashCount > 0)
                {
                    Vector3 normal = collision.contacts[0].normal;
                    Vector3 inCommingDirection = mRigidbody.velocity.normalized;
                    Vector3 reflectDirection = Vector3.Reflect(inCommingDirection, normal);
                    float angle = Mathf.Atan2(reflectDirection.y, reflectDirection.x) - Mathf.Atan2(transform.right.y, transform.right.x);
                    angle *= Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                    mCrashCount--;
                }
                else
                {
                    mStatus = ProjectileStatus.Crash;
                    mAnimator.SetTrigger("Destroy");
                }
            }
        }
    }


    private void OnCollisionStay2D(Collision2D other)
    {
        Debug.Log("Stay" + mCrashCount);
        mCrashCount--;
    }

}
