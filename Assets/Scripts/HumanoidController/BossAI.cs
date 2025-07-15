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

    // Boss״̬ö��
    enum Status
    {
        Idle,       // ��ֹ״̬
        Chasing,    // ׷��״̬
        Rampage     // ����״̬��Ѫ������50%��
    }
    private Status mStatus = Status.Idle;

    // ״̬��ʱ��
    private float mShootPatternTimer = 0f;
    private int mCurrentShootMode = 1; // ��ǰ���ģʽ��1,2,3��

    // ���ò���
    public float chaseDuration = 5f;      // ����״̬׷��ʱ��
    public float rampageChaseDuration = 3f; // ����״̬׷��ʱ��
    public float shootDuration = 15f;      // �������ʱ��

    public bool isRampageMode = false;   // �Ƿ��ڱ���ģʽ
    public bool isChangingMode = false;

    void Update()
    {
        if (mBossStatus.mHealthPoint <= 0 && !isRampageMode)
         {         
            mStatus = Status.Idle;
            isChangingMode = true;
            Debug.Log("Boss��ʼ���뱩��ģʽ��");
         }

        if(mBossStatus.mHealthPoint <mBossStatus.mMaxHealthPoint && isChangingMode)
        {
            mBossStatus.Recover(20);
            Debug.Log("Boss���ڽ��뱩��ģʽ��");
        }
        if (mBossStatus.mHealthPoint == mBossStatus.mMaxHealthPoint && isChangingMode)
        {
            isChangingMode = false;
            Debug.Log("Boss�ɹ����뱩��ģʽ��");
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

        // ���������ң��л���׷��״̬
        if (mSightHandler.isInSight && !isChangingMode)
        {
            mStatus = Status.Chasing;
            mStatusTimer = Time.time;
            Debug.Log("Boss������ң�����׷��״̬");
        }
    }

    private void HandleChasingState(Vector3 relaPos)
    {
        float currentChaseDuration = isRampageMode ? rampageChaseDuration : chaseDuration;

        // ׷��׶�
        if (Time.time - mStatusTimer < currentChaseDuration)
        {
            // ׷�����
            if (relaPos.magnitude > 20f) // ����һ������
            {
                mBehaviorHandler.Move();
            }
            else
            {
                mBehaviorHandler.Idle();
            }
        }
        // ����׶�
        else if (Time.time - mStatusTimer < currentChaseDuration + shootDuration)
        {
            // ֹͣ�ƶ�����ʼ���
            mBehaviorHandler.Idle();

            // �������ģʽ
            BossBehavior bossBehavior = mBehaviorHandler as BossBehavior;
            bossBehavior.mShootMode = mCurrentShootMode;

            // ִ�����
            mBehaviorHandler.Shoot();

            // �������ģʽ��ʱ��
            if (Time.time - mShootPatternTimer > shootDuration / 3f)
            {
                mCurrentShootMode = mCurrentShootMode % 3 + 1; // ѭ��1,2,3
                mShootPatternTimer = Time.time;
                Debug.Log("�л����ģʽ: " + mCurrentShootMode);
            }
        }
        // �ص�׷��׶�
        else
        {
            mStatusTimer = Time.time;
            mCurrentShootMode = 1; // �������ģʽ
        }
    }

    private void HandleRampageState(Vector3 relaPos)
    {
        // ����״̬�³����ƶ������
        if (relaPos.magnitude > 2f)
        {
            mBehaviorHandler.Move();
        }

        // �������ģʽ
        BossBehavior bossBehavior = mBehaviorHandler as BossBehavior;
        bossBehavior.mShootMode = mCurrentShootMode;
        // ִ�����
        mBehaviorHandler.Shoot();

        // �������ģʽ
        if (Time.time - mShootPatternTimer > shootDuration / 3f)
        {
            mCurrentShootMode = mCurrentShootMode % 3 + 1; // ѭ��1,2,3
            mShootPatternTimer = Time.time;
            Debug.Log("����ģʽ���л����ģʽ: " + mCurrentShootMode);
        }
    }
}
