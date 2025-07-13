using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HeroCollide : MonoBehaviour
{
    private Light2D heroLight;
    private static Light2D globalLight;
    // Start is called before the first frame update

    public bool getLegPack = false;
    public bool getOptPack = false;
    public bool getCPUPack = false;
    public bool getHidePack = false;
    void Start()
    {
        heroLight = GetComponentInChildren<Light2D>();
        globalLight = GameObject.Find("GlobalLight").GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject e = collision.gameObject;
        if (e.tag == "LegPack")
        {
            GetComponent<HeroBehavior>().canDash = true;
            LegPackBehaviourScript Leg = collision.gameObject.GetComponent<LegPackBehaviourScript> ();
            if ( Leg != null ) { Leg.Delete(); }
            getLegPack = true;
        }
        else if (e.tag == "BloodPackage")
        {
            GetComponent<HumanoidStatus>().Recover(10);
            BloodPackage bp = collision.gameObject.GetComponent<BloodPackage>();
            bp.Delete();
        }
        else if(e.tag == "OptPack")
        {
            if (heroLight != null && globalLight != null)
            {
                heroLight.intensity = heroLight.intensity + 0.2f;
                globalLight.intensity = globalLight.intensity + 0.2f; 
            }

            OptPackBehaviourScript op = collision.gameObject.GetComponent<OptPackBehaviourScript>();
            op.Delete();
            getOptPack = true;
        }
        else if(e.tag == "CPUPack")
        {
            GetComponent<HeroBehavior>().canMoveSlow = true;
            CPUPackBehavior CPU = collision.gameObject.GetComponent<CPUPackBehavior>();
            CPU.Delete();
            getCPUPack = true;
        }
        else if(e.tag == "HidePack")
        {
            GetComponent<HeroBehavior>().canHide = true;
            HidePackBehavior cl = collision.gameObject.GetComponent<HidePackBehavior>(); ;
            cl.Delete();
            getHidePack = true;
        }
    }
}
