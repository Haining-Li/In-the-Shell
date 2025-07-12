using System.Collections;
using UnityEngine;

public class HeroBehavior : HumanoidBehavior
{
    private float mDashSpeed = 2f;
    public bool canDash = false;
    public bool canMoveSlow = false;
    public bool canHide = false;

    public float slowMotionFactor = 0.5f;      // 时间缩放比例
    private float slowMotionDuration = 5f;      // 技能持续时间（真实时间）
    private float slowMotionCooldown = 10f;     // 技能冷却时间（真实时间）
    private float mOriginalFixedDeltaTime;     // 存储原始固定时间步长
    private bool mIsTimeSlowing = false;       // 当前是否处于技能状态
    private bool mIsOnCooldown = false;        // 技能是否处于冷却中
    public bool IsTimeSlowingActive => mIsTimeSlowing;
    public bool IsOnCooldown => mIsOnCooldown;

    private float hidingDuration = 10f;      // 技能持续时间（真实时间）
    private float hidingCooldown = 60f;      // 技能冷却时间（真实时间）

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

    // 激活时间减缓技能
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
        // 设置透明度为0.5
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color originalColor = renderer.color;
        renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

        // 持续hidingDuration秒
        yield return new WaitForSecondsRealtime(hidingDuration);

        // 恢复透明度
        renderer.color = originalColor;
        mIsHiding = false;

        // 进入冷却
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

    // 修改射击方法 - 使用不受缩放影响的时间
    public override void Shoot()
    {
        mAnimator.SetTrigger("Shoot");
        if (Time.unscaledTime - mShootTimer > mShootRate)
        {
            base.Shoot();
            mShootTimer = Time.unscaledTime;
        }
    }

    // 修改冲刺方法
    public override void Dash()
    {
        if (canDash)
        {
            // 计算实际的冲刺力（考虑时间缩放）
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