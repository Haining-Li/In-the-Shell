using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : HumanoidController
{
    // Start is called before the first frame update
    void Start()
    {
        mBehaviorHandler = GetComponent<HumanoidBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
