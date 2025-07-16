// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Default Settings")]
    public int defaultWeaponType = 1; // 默认武器1
    public int defaultBulletType = 1; // 默认弹药1
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private string[] GunName = {
        "Pistol", "Submachine Gun", "Shotgun", "Sniper Gun"
    };

    public GameObject FixedGunGenerator()
    {

        return null;
    }

    private int gunType;
    public int GunType
    {
        set
        {
            gunType = value;
        }
    }
    private int bulletType;
    public int BulletType
    {
        set
        {
            bulletType = value;
        }
    }

    public GameObject GunGenerator()
    {
        int finalGunType = gunType == 0 ? defaultWeaponType : gunType;
        int finalBulletType = bulletType == 0 ? defaultBulletType : bulletType;
    
        GameObject gun = finalGunType switch
        {
            1 => Instantiate(Resources.Load("Prefabs/Hero/Weapon/Pistol") as GameObject),
            2 => Instantiate(Resources.Load("Prefabs/Hero/Weapon/SubmachineGun") as GameObject),
            3 => Instantiate(Resources.Load("Prefabs/Hero/Weapon/Shotgun") as GameObject),
            4 => Instantiate(Resources.Load("Prefabs/Hero/Weapon/SniperGun") as GameObject),
            _ => null
        };
    
        if (gun != null)
        {
            gun.GetComponent<Weapon>().mProjectile = finalBulletType switch
            {
                1 => Resources.Load("Prefabs/Projectile/BlueLaser") as GameObject,
                2 => Resources.Load("Prefabs/Projectile/ReflectableLaser") as GameObject,
                3 => Resources.Load("Prefabs/Projectile/FragmentingOrb") as GameObject,
                _ => null
            };
        }
    
        return gun;
    }
    
}
