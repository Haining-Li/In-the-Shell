using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStatus : HumanoidStatus
{
    private CombatTimer combatTimer; // 确保已声明这个变量
    AudioController audioController;

    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        combatTimer = FindObjectOfType<CombatTimer>(); // 确保正确初始化
        if (combatTimer == null)
        {
            Debug.LogWarning("CombatTimer not found in scene!");
        }
        audioController = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mIsDying)
        {
            AnimatorStateInfo stateInfo = mAnimator.GetCurrentAnimatorStateInfo(0);
            if (Time.time - mStatusTimer > stateInfo.length)
            {
                Die();
            }
        }
    }

    public override void GetHurt(int damage)
    {
        audioController.PlaySfx(audioController.HeroHurt);
        base.GetHurt(damage);
        Debug.Log(damage + " " + mHealthPoint);
        if (mHealthPoint <= 0 && !mIsDying)
        {
            mIsDying = true;
            mAnimator.SetBool("Die", true);
            mStatusTimer = Time.time;

            // 死亡时立即暂停计时器
            if (combatTimer != null)
            {
                combatTimer.PauseAndResetTimer();
            }
        }
        else
        {
            mAnimator.SetTrigger("GetHurt");
        }
    }

    public override void Die()
    {
        base.Die();
        
        // 死亡面板显示
        GameObject canvas = GameObject.Find("Death");
        audioController.PlaySfx(audioController.HeroDeath);
        if (canvas != null)
        {
            Transform panelTransform = canvas.transform.Find("Panel");
            if (panelTransform != null)
            {
                GameObject panel = panelTransform.gameObject;
                panel.SetActive(true);
            }
        }
    }
}