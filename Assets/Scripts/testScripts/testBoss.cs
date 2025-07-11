using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    private Animator animator;
    private bool isAutoLive = true;  // 自活属性
    private bool isDead = false;     // 用于跟踪死亡状态

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();
        UpdateAnimatorParameters();
    }

    void HandleInput()
    {
        // 切换移动状态 (0/5)
        if (Input.GetKeyDown(KeyCode.R) && !isDead)
        {
            float currentSpeed = animator.GetFloat("Speed");
            animator.SetFloat("Speed", currentSpeed > 0 ? 0 : 5);
        }

        // 切换射击状态开关
        if (Input.GetKeyDown(KeyCode.Space) && !isDead)
        {
            bool currentShooting = animator.GetBool("IsShooting");
            animator.SetBool("IsShooting", !currentShooting);

            // 如果开启射击，循环切换射击类型 (1-3)
            if (!currentShooting)
            {
                int currentType = animator.GetInteger("ShootingType");
                animator.SetInteger("ShootingType", (currentType % 3) + 1);
            }
        }

        // 切换交谈状态
        if (Input.GetKeyDown(KeyCode.T) && !isDead)
        {
            bool currentTalking = animator.GetBool("IsTalking");
            animator.SetBool("IsTalking", !currentTalking);
        }

        // 切换死亡状态 (使用Trigger)
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!isDead)
            {
                // 触发死亡状态
                animator.SetTrigger("IsDead");
                isDead = true;
            }
            else
            {
                // 复活
                isDead = false;
                // 重置其他参数
                animator.ResetTrigger("IsDead"); // 重置触发器
                animator.SetFloat("Speed", 0);
                animator.SetBool("IsShooting", false);
                animator.SetBool("IsTalking", false);
            }
        }

        // 切换自动存活状态
        if (Input.GetKeyDown(KeyCode.A))
        {
            isAutoLive = !isAutoLive;
            if (isAutoLive && isDead)
            {
                // 自动复活
                isDead = false;
                animator.ResetTrigger("IsDead");
            }
        }
    }

    void UpdateAnimatorParameters()
    {
        // 如果处于死亡状态，禁用其他所有参数
        if (isDead)
        {
            animator.SetFloat("Speed", 0);
            animator.SetBool("IsShooting", false);
            animator.SetBool("IsTalking", false);
        }

        // 处理自动复活
        if (isAutoLive && isDead)
        {
            isDead = false;
            animator.ResetTrigger("IsDead");
        }
    }
}