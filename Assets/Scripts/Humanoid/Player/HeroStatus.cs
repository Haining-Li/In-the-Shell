using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStatus : HumanoidStatus
{
    private CombatTimer combatTimer; // 确保已声明这个变量
    // 删除这些重复声明
    private Animator mAnimator;
    private bool mIsDying;
    private float mStatusTimer;
    private int mHealthPoint;

    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        combatTimer = FindObjectOfType<CombatTimer>(); // 确保正确初始化
        if (combatTimer == null)
        {
            Debug.LogWarning("CombatTimer not found in scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mIsDying)
        {
            if (Time.time - mStatusTimer > 1.15f)
            {
                Die();
            }
        }
    }

    public override void GetHurt(int damage)
    {
        base.GetHurt(damage);
        if (mHealthPoint == 0)
        {
            mIsDying = true;
            mAnimator.SetTrigger("Die");
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