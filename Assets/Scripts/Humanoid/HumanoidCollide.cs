using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class HumanoidCollide : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 mCollidePos = Vector3.zero;
    public bool isGetHit = false;
    public Vector3 boxCenter = Vector3.zero;
    void Start()
    {
        boxCenter = GetComponent<Collider2D>().bounds.center;
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject e = collision.gameObject;
        // Debug.Log("Anything");
        // Debug.Log(LayerMask.LayerToName(e.layer));
        if (e.layer == LayerMask.NameToLayer("Projectile"))
        {
            mCollidePos = collision.contacts[0].point;
            boxCenter = GetComponent<Collider2D>().bounds.center;
            isGetHit = true;
            Debug.Log("Calc " + mCollidePos + " " + boxCenter);
        }   
    }

    public Vector3 GetCollide()
    {
        isGetHit = false;
        Debug.Log("GetCollide" + (mCollidePos - boxCenter));
        return mCollidePos - boxCenter;
    }
}
