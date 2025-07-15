using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBehavior : ProjectileBehavior
{
    [Header("Homing Settings")]
    public float rotationSpeed = 200f; // 转向速度
    public float maxHomingAngle = 45f; // 每帧最大转向角度

    private Transform playerTransform;
    private bool hasTarget = false;

    void Awake()
    {
        mStatusTimer = Time.time;
        mStatus = ProjectileStatus.Flying;
        // 追踪子弹通常伤害更高
    }

    void Start()
    {
        Init();
        base.mFlyingDuration = 5f;

        // 查找玩家
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            hasTarget = true;
            Debug.Log("成功锁定玩家目标");
        }
        else
        {
            Debug.LogWarning("未找到玩家对象！请确保玩家有'Player'标签");
        }

        // 根据设置忽略碰撞
        GameObject[] ignores = null;
        if (!Hero)
            ignores = GameObject.FindGameObjectsWithTag("Player");
        if (!Enemy)
            ignores = GameObject.FindGameObjectsWithTag("Enemy");

        if (ignores != null)
        {
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
    }

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
        if (Time.time - mStatusTimer >= mFlyingDuration)
        {
            mStatus = ProjectileStatus.Destroyed;
            return;
        }

        // 持续追踪玩家
        if (hasTarget && playerTransform != null)
        {
            // 计算朝向玩家的方向
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

            // 计算需要旋转的角度
            float angle = Vector2.SignedAngle(transform.right, directionToPlayer);

            // 限制单帧最大转向角度
            angle = Mathf.Clamp(angle, -maxHomingAngle, maxHomingAngle);

            // 逐渐转向玩家
            transform.Rotate(0, 0, angle * rotationSpeed * Time.deltaTime);
        }

        // 沿当前方向移动
        mRigidbody.velocity = transform.right * mSpeed;
    }

    public override void Hit()
    {
        if (Time.time - mStatusTimer < mHitDuration)
        {
            transform.rotation = Quaternion.identity;
            base.Hit();
        }
        else
        {
            transform.rotation = Quaternion.identity;
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

        if ((Hero && e.layer == LayerMask.NameToLayer("Hero")) ||
            (Enemy && e.layer == LayerMask.NameToLayer("Enemy")))
        {
            if (mStatus == ProjectileStatus.Flying)
            {
                mStatusTimer = Time.time;
                mStatus = ProjectileStatus.Crash;
                if (mAnimator != null)
                {
                    mAnimator.SetTrigger("Destroy");
                }

                HumanoidStatus targetStatus = e.GetComponent<HumanoidStatus>();
                if (targetStatus != null && targetStatus.mHealthPoint > 0)
                {
                    targetStatus.GetHurt(mDamage);
                }
            }
        }
        else if (e.layer == LayerMask.NameToLayer("Default")) // 撞到墙壁等默认层
        {
            if (mStatus == ProjectileStatus.Flying)
            {
                mStatusTimer = Time.time;
                mStatus = ProjectileStatus.Crash;
                if (mAnimator != null)
                {
                    mAnimator.SetTrigger("Destroy");
                }
            }
        }
    }
}