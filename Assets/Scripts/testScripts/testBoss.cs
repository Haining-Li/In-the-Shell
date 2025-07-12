using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    private Animator animator;
    private bool isAutoLive = true;
    private bool isDead = false;

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
        if (Input.GetKeyDown(KeyCode.R) && !isDead)
        {
            float currentSpeed = animator.GetFloat("Speed");
            animator.SetFloat("Speed", currentSpeed > 0 ? 0 : 5);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isDead)
        {
            bool currentShooting = animator.GetBool("IsShooting");
            animator.SetBool("IsShooting", !currentShooting);

            if (!currentShooting)
            {
                int currentType = animator.GetInteger("ShootingType");
                animator.SetInteger("ShootingType", (currentType % 3) + 1);
            }
        }

        if (Input.GetKeyDown(KeyCode.T) && !isDead)
        {
            bool currentTalking = animator.GetBool("IsTalking");
            animator.SetBool("IsTalking", !currentTalking);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!isDead)
            {
                animator.SetTrigger("IsDead");
                isDead = true;
            }
            else
            {
                isDead = false;
                animator.ResetTrigger("IsDead");
                animator.SetFloat("Speed", 0);
                animator.SetBool("IsShooting", false);
                animator.SetBool("IsTalking", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            isAutoLive = !isAutoLive;
            if (isAutoLive && isDead)
            {
                isDead = false;
                animator.ResetTrigger("IsDead");
            }
        }
    }

    void UpdateAnimatorParameters()
    {
        if (isDead)
        {
            animator.SetFloat("Speed", 0);
            animator.SetBool("IsShooting", false);
            animator.SetBool("IsTalking", false);
        }

        if (isAutoLive && isDead)
        {
            isDead = false;
            animator.ResetTrigger("IsDead");
        }
    }
}