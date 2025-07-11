using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyBehaviorA : HumanoidBehavior
{
    
    void Start()
    {
        Init();
    }
    void Update()
    {
        
        
    }


    public new void Shoot()
    {
        if (Time.time - mShootTimer > mShootRate)
        {
            base.Shoot();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}