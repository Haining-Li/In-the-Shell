using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
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

    // Boss状态枚举
    enum Status
    {
        Idle,       // 静止状态
        Chasing,    // 追逐状态
        Rampage     // 暴走状态（血量低于50%）
    }
    private Status mStatus = Status.Idle;

    // 状态计时器
    private float mShootPatternTimer = 0f;
    private int mCurrentShootMode = 1; // 当前射击模式（1,2,3）

    // 配置参数
    public float chaseDuration = 5f;      // 正常状态追逐时间
    public float rampageChaseDuration = 3f; // 暴走状态追逐时间
    public float shootDuration = 15f;      // 射击持续时间

    public bool isRampageMode = false;   // 是否处于暴走模式
    public bool isChangingMode = false;

    void Update()
    {
        if (mBossStatus.mHealthPoint <= 0 && !isRampageMode)
         {         
            mStatus = Status.Idle;
            isChangingMode = true;
            Debug.Log("Boss开始进入暴走模式！");
         }

        if(mBossStatus.mHealthPoint <mBossStatus.mMaxHealthPoint && isChangingMode)
        {
            mBossStatus.Recover(20);
            Debug.Log("Boss正在进入暴走模式！");
        }
        if (mBossStatus.mHealthPoint == mBossStatus.mMaxHealthPoint && isChangingMode)
        {
            isChangingMode = false;
            Debug.Log("Boss成功进入暴走模式！");
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

        // 如果发现玩家，切换到追逐状态
        if (mSightHandler.isInSight && !isChangingMode)
        {
            mStatus = Status.Chasing;
            mStatusTimer = Time.time;
            Debug.Log("Boss发现玩家，进入追逐状态");
        }
    }

    private void HandleChasingState(Vector3 relaPos)
    {
        float currentChaseDuration = isRampageMode ? rampageChaseDuration : chaseDuration;

        // 追逐阶段
        if (Time.time - mStatusTimer < currentChaseDuration)
        {
            // 追逐玩家
            if (relaPos.magnitude > 20f) // 保持一定距离
            {
                mBehaviorHandler.Move();
            }
            else
            {
                mBehaviorHandler.Idle();
            }
        }
        // 射击阶段
        else if (Time.time - mStatusTimer < currentChaseDuration + shootDuration)
        {
            // 停止移动，开始射击
            mBehaviorHandler.Idle();

            // 设置射击模式
            BossBehavior bossBehavior = mBehaviorHandler as BossBehavior;
            bossBehavior.mShootMode = mCurrentShootMode;

            // 执行射击
            mBehaviorHandler.Shoot();

            // 更新射击模式计时器
            if (Time.time - mShootPatternTimer > shootDuration / 3f)
            {
                mCurrentShootMode = mCurrentShootMode % 3 + 1; // 循环1,2,3
                mShootPatternTimer = Time.time;
                Debug.Log("切换射击模式: " + mCurrentShootMode);
            }
        }
        // 回到追逐阶段
        else
        {
            mStatusTimer = Time.time;
            mCurrentShootMode = 1; // 重置射击模式
        }
    }

    private void HandleRampageState(Vector3 relaPos)
    {
        // 暴走状态下持续移动并射击
        if (relaPos.magnitude > 2f)
        {
            mBehaviorHandler.Move();
        }

        // 设置射击模式
        BossBehavior bossBehavior = mBehaviorHandler as BossBehavior;
        bossBehavior.mShootMode = mCurrentShootMode;
        // 执行射击
        mBehaviorHandler.Shoot();

        // 更新射击模式
        if (Time.time - mShootPatternTimer > shootDuration / 3f)
        {
            mCurrentShootMode = mCurrentShootMode % 3 + 1; // 循环1,2,3
            mShootPatternTimer = Time.time;
            Debug.Log("暴走模式下切换射击模式: " + mCurrentShootMode);
        }
    }
}
=======

public class BossAI : HumanoidController
{
    // Start is called before the first frame update
    void Start()
    {
        mBehaviorHandler = GetComponent<HumanoidBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
>>>>>>> aaaf91160c78540ef04847ee0c3cc19fc9ba132d
