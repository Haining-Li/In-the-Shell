using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidController : MonoBehaviour
{
    protected HumanoidBehavior mBehaviorHandler = null;
    protected HumanoidStatus mStatusHandler = null;
    protected Sight mSightHandler = null;
    protected float mStatusTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    virtual protected void Init()
    {
        mBehaviorHandler = GetComponent<HumanoidBehavior>();
        mSightHandler = GetComponentInChildren<Sight>();
    }
}
