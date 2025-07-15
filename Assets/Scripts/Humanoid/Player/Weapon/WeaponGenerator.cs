// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{
    // Start is called before the first frame update
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
        if (gunType == 0)
            gunType = Random.Range(1, 4);
        if (bulletType == 0)
            bulletType = Random.Range(1, 4);
        GameObject gun = gunType switch
        {
            1 => Instantiate(Resources.Load("Prefabs/Player/Weapon/Pistol") as GameObject),
            2 => Instantiate(Resources.Load("Prefabs/Player/Weapon/SubmachineGun") as GameObject),
            3 => Instantiate(Resources.Load("Prefabs/Player/Weapon/Shotgun") as GameObject),
            4 => Instantiate(Resources.Load("Prefabs/Player/Weapon/SniperRifle") as GameObject),
            _ => null
        };
        gun.GetComponent<Weapon>().mProjectile = bulletType switch
        {
            1 => Resources.Load("Prefabs/Projectile/BlueLaser") as GameObject,
            2 => Resources.Load("Prefabs/Projectile/BlueOrb") as GameObject,
            3 => Resources.Load("Prefabs/Projectile/ReflectableLaser") as GameObject,
            4 => Resources.Load("Prefabs/Projectile/FragmentingOrb") as GameObject,
            _ => null
        };
        return gun;
    }
    
}
