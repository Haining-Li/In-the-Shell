using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvalBehavior : ProjectileBehavior
{
    void Awake()
    {
        mStatusTimer = Time.time;
        mStatus = ProjectileStatus.Flying;
    }


    // Start is called before the first frame update
    void Start()
    {
        Init();
        mSpeed *= Random.Range(0.8f, 1.2f);
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
                if (e.GetComponent<HumanoidStatus>().mHealthPoint > 0)
                {
                    e.GetComponent<HumanoidStatus>().GetHurt(mDamage);
                }
            }
        }
        else if (e.layer == LayerMask.NameToLayer("Default"))
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
