using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoAI : HumanoidController
{
    // Start is called before the first frame update
    // private bool towardsRight = true;
    [SerializeField] private float mAlertDuration = 5f;
    void Start()
    {
        Init();
    }

    enum Status
    {
        Idle, Alert, Attack
    }
    [SerializeField]
    private Status mStatus = Status.Idle;

    // Update is called once per frame
    void Update()
    {
        if (!isActivated) return;

        Vector3 relaPos = mSightHandler.mTargetPosition - transform.localPosition;

        mBehaviorHandler.mFacingDirection = mBehaviorHandler.mMoveDirection = relaPos;

        if (relaPos.x > 0) towardsRight = true;
        if (relaPos.x < 0) towardsRight = false;

        switch (mStatus)
        {
            case Status.Idle:
                // Debug.Log(mSightHandler.isInSight);
                Idle();
                break;
            case Status.Alert:
                // Debug.Log(mSightHandler.isInSight);
                Alert();
                break;
            case Status.Attack:
                // Debug.Log(mSightHandler.isInSight);
                Attack();
                break;
        }
        FlipX();
    }

    private float mAlertTimer = 0f;
    private float mChangeRate = 1f;
    private bool isRandomMove = false;

    void Idle()
    {
        // Action
        mBehaviorHandler.Idle();
        // Set Status
        if (mSightHandler.isInSight)
        {
            mStatus = Status.Attack;
            mStatusTimer = Time.time;
        }
        if (mCollideHandler.isGetHit)
        {
            mStatus = Status.Alert;
            mBehaviorHandler.mMoveDirection = mBehaviorHandler.mFacingDirection = mCollideHandler.GetCollide();
            mStatusTimer = Time.time;
        }
    }

    void Alert()
    {
        // Action
        mBehaviorHandler.Move();
        mBehaviorHandler.Shoot();

        // Set Status
        if (mSightHandler.isInSight)
        {
            mStatus = Status.Attack;
            mStatusTimer = Time.time;
        }
        if (mCollideHandler.isGetHit)
        {
            Vector3 relaPos = mCollideHandler.GetCollide() - transform.localPosition;
            mBehaviorHandler.mMoveDirection = mBehaviorHandler.mFacingDirection = relaPos;
            mStatusTimer = Time.time;
        }
        else
        {
            if (Time.time - mStatusTimer > mAlertDuration)
            {
                mStatusTimer = Time.time;
                mStatus = Status.Idle;
            }
        }
    }

    private float mWanderTime;
    private Vector3 bias = Vector3.zero;
    void Attack()
    {
        // Action
        float radius = GetComponentInChildren<CircleCollider2D>().radius * transform.localScale.y;
        if (mSightHandler.isInSight)
        {
            Vector3 relaPos = mSightHandler.mTargetPosition - transform.localPosition;
            mBehaviorHandler.mFacingDirection = relaPos;

            float rate = relaPos.magnitude / radius;
            rate = (rate - 0.5f) * 2;
            mBehaviorHandler.mMoveDirection = rate * relaPos + bias;


            if (Time.time - mWanderTime > 0.8f)
            {
                mWanderTime = Time.time;
                float sign = Random.Range(-1f, 1f);
                bias = Quaternion.Euler(0, 0, 90) * (sign * relaPos);
            }

            mBehaviorHandler.Move();
            mBehaviorHandler.Shoot();
        }
        // Set Status
        if (!mSightHandler.isInSight)
        {
            mStatus = Status.Alert;
            mStatusTimer = Time.time;
        }
    }
}
