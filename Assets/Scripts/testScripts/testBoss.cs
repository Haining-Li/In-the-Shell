using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    private Animator animator;
    private bool isAutoLive = true;  // �Ի�����
    private bool isDead = false;     // ���ڸ�������״̬

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
        // �л��ƶ�״̬ (0/5)
        if (Input.GetKeyDown(KeyCode.R) && !isDead)
        {
            float currentSpeed = animator.GetFloat("Speed");
            animator.SetFloat("Speed", currentSpeed > 0 ? 0 : 5);
        }

        // �л����״̬����
        if (Input.GetKeyDown(KeyCode.Space) && !isDead)
        {
            bool currentShooting = animator.GetBool("IsShooting");
            animator.SetBool("IsShooting", !currentShooting);

            // ������������ѭ���л�������� (1-3)
            if (!currentShooting)
            {
                int currentType = animator.GetInteger("ShootingType");
                animator.SetInteger("ShootingType", (currentType % 3) + 1);
            }
        }

        // �л���̸״̬
        if (Input.GetKeyDown(KeyCode.T) && !isDead)
        {
            bool currentTalking = animator.GetBool("IsTalking");
            animator.SetBool("IsTalking", !currentTalking);
        }

        // �л�����״̬ (ʹ��Trigger)
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!isDead)
            {
                // ��������״̬
                animator.SetTrigger("IsDead");
                isDead = true;
            }
            else
            {
                // ����
                isDead = false;
                // ������������
                animator.ResetTrigger("IsDead"); // ���ô�����
                animator.SetFloat("Speed", 0);
                animator.SetBool("IsShooting", false);
                animator.SetBool("IsTalking", false);
            }
        }

        // �л��Զ����״̬
        if (Input.GetKeyDown(KeyCode.A))
        {
            isAutoLive = !isAutoLive;
            if (isAutoLive && isDead)
            {
                // �Զ�����
                isDead = false;
                animator.ResetTrigger("IsDead");
            }
        }
    }

    void UpdateAnimatorParameters()
    {
        // �����������״̬�������������в���
        if (isDead)
        {
            animator.SetFloat("Speed", 0);
            animator.SetBool("IsShooting", false);
            animator.SetBool("IsTalking", false);
        }

        // �����Զ�����
        if (isAutoLive && isDead)
        {
            isDead = false;
            animator.ResetTrigger("IsDead");
        }
    }
}