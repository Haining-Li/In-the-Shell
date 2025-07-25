using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : HumanoidController
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    enum Status
    {
        Idle, Attack
    }
    [SerializeField] private Status mStatus = Status.Idle;
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
                Idle();
                break;
            case Status.Attack:
                Attack();
                break;

        }
        FlipX();
    }


    Vector3 direction = Vector3.zero;
    int move = 0;
    void Idle()
    {
        if (Time.time - mStatusTimer > 2f)
        {
            Debug.Log("Set direction");
            Debug.Log(move);
            move = Random.Range(0, 2);
            Debug.Log(move);
            float moveX = Random.Range(-1f, 1f);
            float moveY = Random.Range(-1f, 1f);
            direction = new Vector3(moveX, moveY);
            mBehaviorHandler.mMoveDirection = direction;
            mStatusTimer = Time.time;
        }
        else
        {
            mBehaviorHandler.mMoveDirection = direction;
        }

        if (move == 1)
        {
            Debug.Log("Move" + direction);
            mBehaviorHandler.Move();
        }
        else
        {
            Debug.Log(move);
            mBehaviorHandler.Idle();
        }

        if (mSightHandler.isInSight)
        {
            mStatus = Status.Attack;
            mStatusTimer = Time.time;
        }
    }

    private float mWanderTime;
    private Vector3 bias = Vector3.zero;
    void Attack()
    {
        float radius = GetComponentInChildren<CircleCollider2D>().radius * transform.localScale.y;
        if (mSightHandler.isInSight)
        {
            Vector3 relaPos = mSightHandler.mTargetPosition - transform.localPosition;
            mBehaviorHandler.mFacingDirection = relaPos;

            float rate = relaPos.magnitude / radius;
            // Debug.Log(relaPos.magnitude + " " + radius + " " + rate);
            mBehaviorHandler.mShootRate = 2f - 1.5f * rate;
            rate = - rate;
            mBehaviorHandler.mMoveDirection = (rate > 0.5 ? 0 : 2 * rate - 1) * relaPos + bias;


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
            mStatus = Status.Idle;
            mStatusTimer = Time.time;
        }
    }
}
