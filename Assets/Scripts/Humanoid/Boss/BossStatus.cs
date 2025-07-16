using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatus : HumanoidStatus
{
    // Start is called before the first frame update
    [SerializeField] private GameObject bloodPackagePrefab;
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
        }
    }

    public override void GetHurt(int damage)
    {
        // Debug.Log(mHP);
        base.GetHurt(damage);
        if (mHealthPoint == 0)
        {
            mIsDying = true;
            mAnimator.SetBool("Die",true);
            mStatusTimer = Time.time;
        } 
        else
        {
            mAnimator.SetTrigger("GetHurt");
        }
    }

    public override void Die()
    {
        BossAI mBossAI = GetComponent<BossAI>();
        if (mBossAI.isRampageMode && mHealthPoint == 0)
        {
            Debug.Log("Die");
            base.Die();
            if (Random.Range(0f, 1f) <= 1f)
            {
                Instantiate(bloodPackagePrefab, transform.position, Quaternion.identity);
            }
        }
    }

    public override void Recover(int health)
    {
        mAnimator.SetBool("Die", false);
        base.Recover(health);
    }
}
