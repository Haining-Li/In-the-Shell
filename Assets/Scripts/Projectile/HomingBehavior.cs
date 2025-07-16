using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBehavior : ProjectileBehavior
{
    [Header("Homing Settings")]
    public float rotationSpeed = 200f;
    public float maxHomingAngle = 45f;

    private Transform playerTransform;
    private bool hasTarget = false;

    void Awake()
    {
        mStatusTimer = Time.time;
        mStatus = ProjectileStatus.Flying;
    }

    void Start()
    {
        Init();
        base.mFlyingDuration = 5f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            hasTarget = true;
        }

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

        if (hasTarget && playerTransform != null)
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

            float angle = Vector2.SignedAngle(transform.right, directionToPlayer);

            angle = Mathf.Clamp(angle, -maxHomingAngle, maxHomingAngle);

            transform.Rotate(0, 0, angle * rotationSpeed * Time.deltaTime);
        }

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
        else if (e.layer == LayerMask.NameToLayer("Default"))
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