using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbEnemyAI : HumanoidController
{
    // Start is called before the first frame update
    private bool towardsRight = true;
    void Start()
    {
        Init();
    }

    enum Status
    {
        Idle, Alert, Chasing
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
                mBehaviorHandler.Idle();
                if (mSightHandler.isInSight)
                    mStatus = Status.Chasing;
                break;
            case Status.Chasing:
                if (relaPos.magnitude > 40f)
                    mBehaviorHandler.Move();
                if (mSightHandler.isInSight)
                    mBehaviorHandler.Shoot();
                else
                {
                    mStatus = Status.Alert;
                    mStatusTimer = Time.time;
                }
                break;
            case Status.Alert:
                if (Time.time - mStatusTimer < 5f)
                    mBehaviorHandler.Move();
                if (mSightHandler.isInSight)
                    mStatus = Status.Chasing;
                else
                    mStatus = Status.Idle;
                break;
        }

        GetComponent<SpriteRenderer>().flipX = !towardsRight;

    }
}
