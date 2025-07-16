using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    public int mShotNum = 6;
    public float mSectorRange = 30f;

    public override void Shoot()
    {

        if (Time.time - mShootTimer > mShootRate)
        {
            mShootTimer = Time.time;

            Vector3 origin = mTowards;
            for (int i = 0; i < mShotNum; i++)
            {
                float angle = Random.Range(-mSectorRange, mSectorRange);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                mTowards = rotation * origin;
                base.Shoot();
            }
            mTowards = origin;
            isActivate = false;
            parentRigid.AddForce(-150 * mTowards, ForceMode2D.Impulse);
        }
    }
}
