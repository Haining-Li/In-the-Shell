using System.Collections;
using UnityEngine;

public class HeroBehavior : HumanoidBehavior
{
    public bool canDash = false;
    public bool canMoveSlow = false;
    public bool canHide = false;

    public float slowMotionFactor = 0.5f;      // ʱ�����ű���
    private float slowMotionDuration = 5f;      // ���ܳ���ʱ�䣨��ʵʱ�䣩
    private float slowMotionCooldown = 10f;     // ������ȴʱ�䣨��ʵʱ�䣩
    private float mOriginalFixedDeltaTime;     // �洢ԭʼ�̶�ʱ�䲽��
    private bool mIsTimeSlowing = false;       // ��ǰ�Ƿ��ڼ���״̬
    private bool mIsOnCooldown = false;        // �����Ƿ�����ȴ��
    public bool IsTimeSlowingActive => mIsTimeSlowing;
    public bool IsOnCooldown => mIsOnCooldown;

    private float hidingDuration = 10f;      // ���ܳ���ʱ�䣨��ʵʱ�䣩
    private float hidingCooldown = 60f;      // ������ȴʱ�䣨��ʵʱ�䣩

    private bool mIsHiding = false;
    private bool mIsOnHidingCoolDown = false;
    float mHideTimer = 0f;

    public bool IsHiding => mIsHiding;
    public bool IsHidingOnCooldown => mIsOnHidingCoolDown;

    public delegate void TimeSlowChangedEvent(bool isActive, float duration);
    public event TimeSlowChangedEvent OnTimeSlowChanged;

    void Start()
    {
        Init();
        mFirePoint = new Vector3(0, 8, 0);
        mOriginalFixedDeltaTime = Time.fixedDeltaTime;
        mShootTimer = Time.unscaledTime;
        mHideTimer = Time.unscaledTime;
    }

    // ����ʱ���������
    public void ActivateTimeSlow()
    {
        if (!mIsTimeSlowing && canMoveSlow && !mIsOnCooldown)
        {
            StartCoroutine(TimeSlowCoroutine());
        }
    }

    public void Hide()
    {
        if (!mIsHiding && canHide && !mIsOnHidingCoolDown)
        {
            StartCoroutine(HideCoroutine());
        }
    }

    private IEnumerator HideCoroutine()
    {
        mIsHiding = true;
        // ����͸����Ϊ0.5
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color originalColor = renderer.color;
        renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

        // ����hidingDuration��
        yield return new WaitForSecondsRealtime(hidingDuration);

        // �ָ�͸����
        renderer.color = originalColor;
        mIsHiding = false;

        // ������ȴ
        StartCoroutine(HideCooldownCoroutine());
    }

    private IEnumerator HideCooldownCoroutine()
    {
        mIsOnHidingCoolDown = true;
        mHideTimer = Time.unscaledTime;

        while (Time.unscaledTime - mHideTimer < hidingCooldown)
        {
            yield return null;
        }

        mIsOnHidingCoolDown = false;
    }

    private IEnumerator TimeSlowCoroutine()
    {
        mIsTimeSlowing = true;
        OnTimeSlowChanged?.Invoke(true, slowMotionDuration);

        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = mOriginalFixedDeltaTime * slowMotionFactor;
        mAnimator.speed = 1f / slowMotionFactor;

        yield return new WaitForSecondsRealtime(slowMotionDuration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = mOriginalFixedDeltaTime;
        mAnimator.speed = 1f;
        mIsTimeSlowing = false;

        OnTimeSlowChanged?.Invoke(false, slowMotionCooldown);
        StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        mIsOnCooldown = true;

        float startTime = Time.unscaledTime;
        float remainingTime = slowMotionCooldown;

        while (remainingTime > 0)
        {
            remainingTime = slowMotionCooldown - (Time.unscaledTime - startTime);
            yield return null;
        }

        mIsOnCooldown = false;

    }

    public override void Move()
    {
        if (mIsTimeSlowing)
        {
            float tempSpeed = mSpeed / slowMotionFactor;
            float force = tempSpeed * mRigidBody.drag;
            mRigidBody.AddForce(force * mDirection);
        }
        else
        {
            base.Move();
        }
        mAnimator.SetFloat("MoveSpeed", 1f);
    }

    // �޸�������� - ʹ�ò�������Ӱ���ʱ��
    public override void Shoot()
    {
        mAnimator.SetTrigger("Shoot");
        if (Time.unscaledTime - mShootTimer > mShootRate)
        {
            base.Shoot();
            mShootTimer = Time.unscaledTime;
        }
    }

    // �޸ĳ�̷���
    public override void Dash()
    {
        if (canDash)
        {
            // ����ʵ�ʵĳ����������ʱ�����ţ�
            Vector3 effectiveForce = mIsTimeSlowing ?
                20 * mSpeed * mDirection / slowMotionFactor :
                20 * mSpeed * mDirection;

            mRigidBody.AddForce(effectiveForce, ForceMode2D.Impulse);
        }
    }

    public override void Idle()
    {
        base.Idle();
        mAnimator.SetFloat("MoveSpeed", 0f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ActivateTimeSlow();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Hide();
        }
    }
}