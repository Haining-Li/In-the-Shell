using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HeroBehavior : HumanoidBehavior
{
    
    public bool canDash = false;
    public bool canMoveSlow = false;
    public bool canHide = false;

    public float slowMotionFactor = 0.5f;
    private float slowMotionDuration = 5f;
    private float slowMotionCooldown = 10f;
    private float dashCooldown = 1f;
    private float mOriginalFixedDeltaTime;
    private bool mIsTimeSlowing = false;
    private bool mIsOnCooldown = false;
    public bool IsTimeSlowingActive => mIsTimeSlowing;
    public bool IsOnCooldown => mIsOnCooldown;
    public bool IsInvincible = false;

    private float hidingDuration = 10f;
    private float hidingCooldown = 60f;

    public bool mIsHiding = false;
    private bool mIsOnHidingCoolDown = false;
    float mHideTimer = 0f;
    float mDashTimer = 0f;

    public bool IsHiding => mIsHiding;
    public bool IsHidingOnCooldown => mIsOnHidingCoolDown;

    public delegate void TimeSlowChangedEvent(bool isActive, float duration);
    public event TimeSlowChangedEvent OnTimeSlowChanged;

    private Rigidbody2D rb2D;
    public Vector3 HeroFirePoint;

    void Start()
    {
        Init();
        // mFirePoint = new Vector3(0, 8, 0);
        originSpeed = mSpeed;
        mWeaponHandler = GetComponentInChildren<Weapon>();
        if (mWeaponHandler == null)
        {
            WeaponGenerator generator = FindObjectOfType<WeaponGenerator>();
            GameObject defaultWeapon = generator.GunGenerator();
            defaultWeapon.transform.SetParent(transform);
            defaultWeapon.transform.localPosition = Vector3.zero;
            mWeaponHandler = defaultWeapon.GetComponent<Weapon>();
        }
        mOriginalFixedDeltaTime = Time.fixedDeltaTime;
        mShootTimer = Time.unscaledTime;
        mHideTimer = Time.unscaledTime;
        rb2D = GetComponent<Rigidbody2D>();
        HeroFirePoint = new Vector3 (8f, 8f, 0f);
    }
    public void ActivateTimeSlow()
    {
        if (!mIsTimeSlowing && canMoveSlow && !mIsOnCooldown)
        {
            AudioController.Instance.PlaySfx(AudioController.Instance.TimeSlow);
            StartCoroutine(TimeSlowCoroutine());
        }
    }

    public void Hide()
    {
        if (!mIsHiding && canHide && !mIsOnHidingCoolDown)
        {
            AudioController.Instance.PlaySfx(AudioController.Instance.Hide);
            StartCoroutine(HideCoroutine());
        }
    }

    private IEnumerator HideCoroutine()
    {
        mIsHiding = true;
        
        GetComponent<URPTransparencyController>().FadeTo(0.2f);

        yield return new WaitForSecondsRealtime(hidingDuration);

        GetComponent<URPTransparencyController>().FadeTo(1f);
        mIsHiding = false;

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
        originSpeed = mSpeed;
        mSpeed /= slowMotionFactor;

        yield return new WaitForSecondsRealtime(slowMotionDuration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = mOriginalFixedDeltaTime;
        mAnimator.speed = 1f;
        mIsTimeSlowing = false;
        mSpeed = originSpeed;

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

    private float originSpeed;
    public override void Move()
    {
        base.Move();
        mAnimator.SetFloat("MoveSpeed", 1f);
    }

    public Weapon mWeaponHandler = null;
    public override void Shoot()
    {
        mAnimator.SetTrigger("Shoot");
        if (Time.unscaledTime - mShootTimer > mShootRate)
        {
            mShootTimer = Time.unscaledTime;
        }

        mWeaponHandler.mFirePoint = mFirePoint;
        mWeaponHandler.mTowards = mTowards;
        mWeaponHandler.Shoot();
    }



    public override void Dash()
    {

        if (canDash && Time.unscaledTime - mDashTimer > dashCooldown)
        {
            AudioController.Instance.PlaySfx(AudioController.Instance.Dash);
            /*
            Vector3 effectiveForce = mIsTimeSlowing ?
                5 * mSpeed * mDirection / slowMotionFactor :
                5 * mSpeed * mDirection;

            mRigidBody.AddForce(effectiveForce, ForceMode2D.Impulse);
            mDashTimer = Time.unscaledTime;
            */
            base.Dash();
        }
    }

    public override void Idle()
    {
        base.Idle();
        mAnimator.SetFloat("MoveSpeed", 0f);
    }

    void Update()
    {
        Vector2 velocity = rb2D.velocity;
        float speed = velocity.magnitude; // 直接获取速度的大小（速率）

        if (speed > mSpeed)
        {
            gameObject.layer = LayerMask.NameToLayer("Projectile");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Hero");
        }

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