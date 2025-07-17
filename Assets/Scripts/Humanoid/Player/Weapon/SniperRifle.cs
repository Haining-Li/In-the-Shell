using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : Weapon
{
    public override void Shoot()
    {
        if (Time.time - mShootTimer > mShootRate)
        {
            mShootTimer = Time.time;
            base.Shoot();
            isActivate = false;
            parentRigid.AddForce(-250 * mTowards, ForceMode2D.Impulse);
        }
    }
    
}
