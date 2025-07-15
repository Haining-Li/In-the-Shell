using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BossBehavior : HumanoidBehavior
{
    // Start is called before the first frame update
    Animator mLowerBody = null;
    public int mShootMode = 1;

    private Transform playerTransform;

    public GameObject mBullet1 = null;
    public GameObject mBullet3 = null;
    public GameObject mBullet3_2 = null;
    public Vector3 mFirePoint1;
    public Vector3 mFirePoint3;
    public float mShootRate1;
    public float mShootRate2;
    public float mShootRate3;
    float mDashTimer=0;
    float mDashRate=0;
    BossAI bossAI = null;

    private Rigidbody2D rb2D;
    private Sight mSightHandler;

    void Start()
    {
        Init();
        mDashRate = mShootRate2;
        rb2D = GetComponent<Rigidbody2D>();
        mLowerBody = GetComponentsInChildren<Animator>()[1];
        mSightHandler = GetComponentInChildren<Sight>();
        bossAI = GetComponent<BossAI>();
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
        if(Time.time - mDashTimer > mDashRate)
        {
            Debug.Log("Dash");
            mDashTimer = Time.time;
            mDirection = (mSightHandler.mTargetPosition - transform.position).normalized;
            Vector3 effectiveForce = 20 * mSpeed * mDirection;
            mRigidBody.AddForce(effectiveForce, ForceMode2D.Impulse);
        }
    }

    public override void Shoot()
    {
        if (Time.time - mShootTimer > mShootRate)
        {
            mShootTimer = Time.time;
            if (mShootMode == 1)//火箭
            {
                mBullet = mBullet1;
                mFirePoint = mFirePoint1;
                if(mDirection.x < 0) { mFirePoint.x = -mFirePoint1.x; }
                base.Shoot();
                mAnimator.SetTrigger("Shoot");
                mAnimator.SetInteger("ShootMode", mShootMode);
                mShootRate = mShootRate1;
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
                if (bossAI.isRampageMode)
                {
                    mBullet = mBullet3_2;
                    float angle = 360 / 6;
                    mShootRate3 = 0.4f;
                    Quaternion rotation = Quaternion.Euler(0, 0, angle);
                    Vector3 origin = mTowards;
                    for (int i = 0; i < 6; i++)
                    {
                        if (mDirection.x < 0) { mFirePoint.x = -mFirePoint3.x; }
                        base.Shoot();
                        mTowards = rotation * mTowards;
                    }
                    mTowards = origin;
                }
                else
                {
                    if (mDirection.x < 0) { mFirePoint.x = -mFirePoint3.x; }
                    base.Shoot();
                }
                mAnimator.SetTrigger("Shoot");
                mAnimator.SetInteger("ShootMode", mShootMode);
                mShootRate = mShootRate3;
            }

        }
        
    }

    public float GetSpeed()
    {
        Vector2 velocity = rb2D.velocity;
        float speed = velocity.magnitude;
        return speed;
    }
       
    public void Update()
    {
        mDirection = (playerTransform.localPosition - transform.localPosition).normalized;
    }
}
