using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmachineGun : Weapon
{
    public int mBurstNum;
    private int mBurstCount = 0;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
    }

    public override void Shoot()
    {
        
        if (Time.time - mCoolDownTimer > mShootRate)
        {
            if (mBurstCount < mBurstNum)
            {
                if (Time.time - mShootTimer > mShootCoolDown)
                {
                    mShootTimer = Time.time;
                    base.Shoot();
                    mBurstCount++;
                }
            }
            else
            {
                isActivate = false;
                mCoolDownTimer = Time.time;
                mBurstCount = 0;
            }
        }
    }

}
