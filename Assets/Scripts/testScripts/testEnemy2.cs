using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private bool isActivated = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        ResetAllParameters();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleActionInput();
    }

    private void HandleMovementInput()
    {
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
        if (Input.GetKeyDown(KeyCode.Alpha2) && !animator.GetBool("IsDead"))
        {
            TriggerDeath();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ToggleActivation();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && isActivated)
        {
            TriggerShooting();
        }
    }

    private void TriggerDeath()
    {
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
    }
}