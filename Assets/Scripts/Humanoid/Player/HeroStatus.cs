using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStatus : HumanoidStatus
{
    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
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
            else
            {
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
        }
        else
        {
            mAnimator.SetTrigger("GetHurt");
        }
    }

    public override void Die()
    {
        base.Die();
    
        
        GameObject canvas = GameObject.Find("Death");
        Transform panelTransform = canvas.transform.Find("Panel");
        GameObject panel = panelTransform.gameObject;
        panel.SetActive(true);
        
    }
}
