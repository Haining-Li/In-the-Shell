using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Status : HumanoidStatus
{
    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void GetHurt(int damage)
    {
        base.GetHurt(damage);
        mAnimator.SetTrigger("GetHurt");
    }

    public override void Die()
    {
        mAnimator.SetTrigger("Die");
        base.Die();
    }
}
