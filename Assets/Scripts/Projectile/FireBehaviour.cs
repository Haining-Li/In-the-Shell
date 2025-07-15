using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehaviour : MonoBehaviour
{
    public int mDamage;
    public BossBehavior mBossBehavior;
    // Start is called before the first frame update
    void Start()
    {
        mBossBehavior = GetComponentInParent<BossBehavior>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject e = collision.gameObject;
        if (e.tag == "Player" && mBossBehavior.mShootMode == 2 && mBossBehavior.GetSpeed()>=mBossBehavior.mSpeed) 
        {
            Debug.Log("Hurt");
            e.GetComponent<HeroStatus>().GetHurt(mDamage);
        }
    }
}
