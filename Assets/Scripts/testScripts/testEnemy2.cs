using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private bool isActivated = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        // 确保所有参数初始状态正确
        ResetAllParameters();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleActionInput();
    }

    private void HandleMovementInput()
    {
        // 按住1键跑步，松开停止
        if (Input.GetKey(KeyCode.Alpha1))
        {
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    private void HandleActionInput()
    {
        // 2键触发死亡（只能触发一次）
        if (Input.GetKeyDown(KeyCode.Alpha2) && !animator.GetBool("IsDead"))
        {
            TriggerDeath();
        }

        // 3键切换激活状态
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ToggleActivation();
        }

        // 4键触发射击（需要已激活状态）
        if (Input.GetKeyDown(KeyCode.Alpha4) && isActivated)
        {
            TriggerShooting();
        }
    }

    private void TriggerDeath()
    {
        // 死亡时禁用其他所有状态
        ResetAllParameters();
        animator.SetBool("IsDead", true);
        Debug.Log("Enemy died");
    }

    private void ToggleActivation()
    {
        isActivated = !isActivated;
        animator.SetBool("IsActivated", isActivated);
        animator.SetTrigger(isActivated ? "IsActivating" : "IsDeactivating");
        Debug.Log("Enemy " + (isActivated ? "activated" : "deactivated"));
    }

    private void TriggerShooting()
    {
        if (!animator.GetBool("IsDead"))
        {
            animator.SetTrigger("IsShooting");
            Debug.Log("Enemy shooting");
        }
    }

    private void ResetAllParameters()
    {
        animator.SetFloat("Speed", 0f);
        animator.SetBool("IsActivated", false);
        animator.SetBool("IsDead", false);
        // 注意：Triggers需要在Animator中自动重置
    }
}