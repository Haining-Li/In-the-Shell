using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class BossAI : HumanoidController
{
    private BossStatus mBossStatus;
    Animator mLowerBody = null;
    void Start()
    {
        mLowerBody = GetComponentsInChildren<Animator>()[1];
        mStatusTimer = 0f;
        mBossStatus = GetComponent<BossStatus>();
        Init();
    }

    enum Status
    {
        Idle,
        Chasing,
        Rampage
    }
    private Status mStatus = Status.Idle;

    private float mShootPatternTimer = 0f;
    private int mCurrentShootMode = 1;

    public float chaseDuration = 5f;
    public float rampageChaseDuration = 3f;
    public float shootDuration = 15f;

    public bool isRampageMode = false;
    public bool isChangingMode = false;

    void Update()
    {
        if (mBossStatus.mHealthPoint <= 0 && !isRampageMode)
         {         
            mStatus = Status.Idle;
            isChangingMode = true;
         }

        if(mBossStatus.mHealthPoint <mBossStatus.mMaxHealthPoint && isChangingMode)
        {
            mBossStatus.Recover(50);
        }
        if (mBossStatus.mHealthPoint == mBossStatus.mMaxHealthPoint && isChangingMode)
        {
            isChangingMode = false;
            mStatus = Status.Rampage;
            isRampageMode=true;
        }

        Vector3 relaPos = mSightHandler.mTargetPosition - transform.localPosition;
        mBehaviorHandler.mFacingDirection = mBehaviorHandler.mMoveDirection = relaPos;
        Vector3 facing = mBehaviorHandler.mFacingDirection = mSightHandler.mTargetPosition - transform.localPosition;
        if (relaPos.x > 0) towardsRight = true;
        if (relaPos.x < 0) towardsRight = false;

        switch (mStatus)
        {
            case Status.Idle:
                HandleIdleState(relaPos);
                break;
            case Status.Chasing:
                HandleChasingState(relaPos);
                break;
            case Status.Rampage:
                HandleRampageState(relaPos);
                break;
        }

        if (facing.x > 0) towardsRight = true;
        if (facing.x < 0) towardsRight = false;

        FlipX();
    }

    private void HandleIdleState(Vector3 relaPos)
    {
        mBehaviorHandler.Idle();

        if (mSightHandler.isInSight && !isChangingMode)
        {
            mStatus = Status.Chasing;
            mStatusTimer = Time.time;
        }
    }

    private void HandleChasingState(Vector3 relaPos)
    {
        float currentChaseDuration = isRampageMode ? rampageChaseDuration : chaseDuration;

        if (Time.time - mStatusTimer < currentChaseDuration)
        {
            if (relaPos.magnitude > 20f)
                mBehaviorHandler.Move();
            }
            else
            {
                mBehaviorHandler.Idle();
            }
        }
        else if (Time.time - mStatusTimer < currentChaseDuration + shootDuration)
        {
            mBehaviorHandler.Idle();

            BossBehavior bossBehavior = mBehaviorHandler as BossBehavior;
            bossBehavior.mShootMode = mCurrentShootMode;

            mBehaviorHandler.Shoot();

            if (Time.time - mShootPatternTimer > shootDuration / 3f)
            {
                mCurrentShootMode = mCurrentShootMode % 3 + 1;
                mShootPatternTimer = Time.time;
            }
        }
        else
        {
            mStatusTimer = Time.time;
            mCurrentShootMode = 1;
        }
    }

    private void HandleRampageState(Vector3 relaPos)
    {
        if (relaPos.magnitude > 2f)
        {
            mBehaviorHandler.Move();
        }

        BossBehavior bossBehavior = mBehaviorHandler as BossBehavior;
        bossBehavior.mShootMode = mCurrentShootMode;
        mBehaviorHandler.Shoot();

        if (Time.time - mShootPatternTimer > shootDuration / 3f)
        {
            mCurrentShootMode = mCurrentShootMode % 3 + 1;
            mShootPatternTimer = Time.time;
        }
    }
}
