using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidController : MonoBehaviour
{

    protected bool towardsRight = true;
    protected HumanoidBehavior mBehaviorHandler = null;
    protected HumanoidStatus mStatusHandler = null;
    protected Sight mSightHandler = null;
    protected float mStatusTimer = 0f;
    protected bool isActivated = false;

    virtual protected void Init()
    {
        mBehaviorHandler = GetComponent<HumanoidBehavior>();
        mSightHandler = GetComponentInChildren<Sight>();
    }

    virtual protected void FlipX()
    {
        Vector3 scale = transform.localScale;
        if (towardsRight)
        {
            scale.x = Math.Abs(scale.x);
        }
        else
        {
            scale.x = -Math.Abs(scale.x);
        }
        transform.localScale = scale;
    }

    virtual public void Activate()
    {
        isActivated = true;
    }
}
