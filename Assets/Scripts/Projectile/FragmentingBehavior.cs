using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentingBehavior : ProjectileBehavior
{

    public int mFragmentCount = 1;
    public int mFragmentNum = 5;
    public float mSubDamageRate = 0.5f;
    private float bias = 5f;
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
                if (e.GetComponent<HumanoidStatus>().mHealthPoint > 0)
                {
                    e.GetComponent<HumanoidStatus>().GetHurt(mDamage);
                }
                Fragmenting();
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

    void Fragmenting()
    {
        if (mFragmentCount > 0)
        {
            for (int i = 0; i < mFragmentNum; i++)
            {
                GameObject sub = Instantiate(gameObject);
                FragmentingBehavior subBehavior = sub.GetComponent<FragmentingBehavior>();
                subBehavior.mFragmentCount = mFragmentCount - 1;
                subBehavior.mDamage = (int)(mDamage * mSubDamageRate);
                float angle = Random.Range(0f, 360f);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                sub.transform.rotation *= rotation;
                sub.transform.localPosition = transform.localPosition + bias * sub.transform.right;
                sub.transform.localScale *= 0.8f;
            }
        }
    }


}
