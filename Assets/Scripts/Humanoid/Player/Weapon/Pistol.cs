using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
    }

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
