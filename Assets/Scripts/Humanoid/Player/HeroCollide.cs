using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCollide : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject e = collision.gameObject;
        if (e.tag == "EnemyBullet")
        {
            
            GetComponent<HumanoidStatus>().GetHurt(10);
        }
        else if (e.tag == "LegPack")
        {
            GetComponent<HeroBehavior>().canDash = true;
            LegPackBehaviourScript Leg = collision.gameObject.GetComponent<LegPackBehaviourScript> ();
            if ( Leg != null ) { Leg.Delete(); }
        }
        else if (e.tag == "BloodPackage")
        {
            GetComponent<HumanoidStatus>().Recover(10);
            BloodPackage bp = collision.gameObject.GetComponent<BloodPackage>();
            bp.Delete();
        }
    }
}
