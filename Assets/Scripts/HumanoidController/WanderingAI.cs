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
                if (mSightHandler.isInSight)
                {
                    mStatus = Status.Attack;
                    mStatusTimer = Time.time;
                }
                break;
            case Status.Attack:
                Attack();
                
                break;

        }
        FlipX();
    }


    Vector3 direction = Vector3.zero;
    void Idle()
    {
        if (Time.time - mStatusTimer > 2f)
        {
            int move = Random.Range(0, 1);
            float moveX = Random.Range(0f, 1f);
            float moveY = Random.Range(0f, 1f);
            direction = new Vector3(moveX, moveY);
            direction = move * direction;
            mStatusTimer = Time.time;
        }
        else
        {
            mBehaviorHandler.mMoveDirection = direction;
        }
    }

    void Attack()
    {
        if (Time.time - mStatusTimer < 5f)
        {

        }
        else
        {
            
        }
    }
}
