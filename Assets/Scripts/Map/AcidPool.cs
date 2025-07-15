using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPool : MonoBehaviour
{
    [SerializeField] private int AcidDamage = 5;
    [SerializeField] private float HurtRate = 0.5f;
    private float mTimer;

    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject e = collision.gameObject;
        if (e.CompareTag("Player")&& !e.GetComponent<HeroBehavior>().IsHiding)
        {
            e.GetComponent<HumanoidStatus>().GetHurt(AcidDamage);
        }
    }
    */
    

    void OnTriggerStay2D(Collider2D other)
    {
        GameObject e = other.gameObject;
        if (e.CompareTag("Player"))
        {
            if (Time.time - mTimer > HurtRate)
            {
                // Debug.Log("Sour");
                e.GetComponent<HumanoidStatus>().GetHurt(AcidDamage);
                mTimer = Time.time;
            }
        }
    }

    /*
    void OnTriggerExit2D(Collider2D collision)
    {
        GameObject e = collision.gameObject;
        if (e.CompareTag("Player"))
        {
            e.GetComponent<HumanoidStatus>().GetHurt(1);
        }
    }
    */
}
