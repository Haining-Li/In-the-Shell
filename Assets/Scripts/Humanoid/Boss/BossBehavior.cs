using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BossBehavior : HumanoidBehavior
{
    // Start is called before the first frame update
    Animator mLowerBody = null;
    public int mShootMode = 2;

    private Transform playerTransform;

    public GameObject mBullet1 = null;
    public GameObject mBullet3 = null;
    public Vector3 mFirePoint1;
    public Vector3 mFirePoint3;

    private Rigidbody2D rb2D;

    void Start()
    {
        Init();
        rb2D = GetComponent<Rigidbody2D>();
        mLowerBody = GetComponentsInChildren<Animator>()[1];
        Debug.Assert(mLowerBody != null, "No Lowerbody Animator");
        Debug.Assert(mAnimator != null);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            Debug.Log("成功锁定玩家目标");
        }
        else
        {
            Debug.LogWarning("未找到玩家对象！请确保玩家有'Player'标签");
        }
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

    public override void Dash()
    {
        mDirection = (playerTransform.position - transform.position).normalized;
        Vector3 effectiveForce = 20 * mSpeed * mDirection;
        mRigidBody.AddForce(effectiveForce, ForceMode2D.Impulse);
    }

    public override void Shoot()
    {
        if (Time.time - mShootTimer > mShootRate)
        {
            mShootTimer = Time.time;
            if(mShootMode == 1)
            {
                mBullet = mBullet1;
                mFirePoint = mFirePoint1;
                base.Shoot();
                mAnimator.SetTrigger("Shoot");
                mAnimator.SetInteger("ShootMode", mShootMode);
            }
            else if (mShootMode == 2)
            {
                Dash();
                mAnimator.SetTrigger("Shoot");
                mAnimator.SetInteger("ShootMode", mShootMode);
            }
            else if(mShootMode == 3)
            {
                mBullet = mBullet3;
                mFirePoint = mFirePoint3;
                base.Shoot();
                mAnimator.SetTrigger("Shoot");
                mAnimator.SetInteger("ShootMode", mShootMode);
            }

        }
        
    }

    public float GetSpeed()
    {
        Vector2 velocity = rb2D.velocity;
        float speed = velocity.magnitude;
        return speed;
    }
}
