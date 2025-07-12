using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1AI : HumanoidController
{
    // Start is called before the first frame update
    private bool towardsRight = true;
    [SerializeField] private float mAlertDuration = 5f;
    void Start()
    {
        Init();
    }

    enum Status
    {
        Idle, Alert, Attack
    }
    private Status mStatus = Status.Idle;

    // Update is called once per frame
    void Update()
    {
        Vector3 relaPos = mSightHandler.mTargetPosition - transform.localPosition;

        mBehaviorHandler.mFacingDirection = mBehaviorHandler.mMoveDirection = relaPos;

        if (relaPos.x > 0) towardsRight = true;
        if (relaPos.x < 0) towardsRight = false;

        switch (mStatus)
        {
            case Status.Idle:
                if (mSightHandler.isInSight)
                {
                    mStatus = Status.Attack;
                    mStatusTimer = Time.time;
                }
                break;
            case Status.Alert:
                if (mSightHandler.isInSight)
                {
                    mStatus = Status.Attack;
                    mStatusTimer = Time.time;
                }
                else
                {
                    Alert();
                    if (Time.time - mStatusTimer > mAlertDuration)
                    {
                        mStatusTimer = Time.time;
                        mStatus = Status.Idle;
                    }
                }
                break;
            case Status.Attack:
                Attack();
                if (!mSightHandler.isInSight)
                {
                    mStatus = Status.Alert;
                    mStatusTimer = Time.time;
                }
                break;
        }

        GetComponent<SpriteRenderer>().flipX = !towardsRight;

    }

    void Alert()
    {
        
    }

    void Attack()
    {
        if (mSightHandler.isInSight)
        {
            Vector3 relaPos = mSightHandler.mTargetPosition - transform.localPosition;
            mBehaviorHandler.mFacingDirection = relaPos;
            if (relaPos.magnitude > 60f)
                mBehaviorHandler.mMoveDirection = relaPos;
            else if (relaPos.magnitude < 40f)
                mBehaviorHandler.mFacingDirection = -relaPos;
            else
                mBehaviorHandler.mFacingDirection = Vector3.zero;
            mBehaviorHandler.Move();
            mBehaviorHandler.Shoot();
        }
    }
}
