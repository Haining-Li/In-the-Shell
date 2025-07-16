using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public override void Shoot()
    {
        
        if (Time.time - mShootTimer > mShootRate)
        {
            mShootTimer = Time.time;
            base.Shoot();
            isActivate = false;
        }
    }
}
