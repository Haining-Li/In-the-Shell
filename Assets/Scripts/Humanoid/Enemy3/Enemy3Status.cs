using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Status : HumanoidStatus
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
            AnimatorStateInfo stateInfo = mAnimator.GetCurrentAnimatorStateInfo(0);
            if (Time.time - mStatusTimer > stateInfo.length)
            {
                Die();
            }
        }
    }

    public override void GetHurt(int damage)
    {
        // Debug.Log(mHP);
        base.GetHurt(damage);
        if (mHealthPoint == 0 && !mIsDying)
        {
            mIsDying = true;
            mAnimator.SetBool("Die", true);
            mStatusTimer = Time.time;
        }
        else
        {
            mAnimator.SetTrigger("GetHurt");
        }
    }

    public override void Die()
    {
        AudioController.Instance.PlaySfx(AudioController.Instance.Enemy3Death);
        base.Die();
        if (Random.Range(0f, 1f) <= 1f)
        {
            Instantiate(bloodPackagePrefab, transform.position, Quaternion.identity);
        }
    }
}
