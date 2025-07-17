using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class HumanoidCollide : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 mCollidePos = Vector3.zero;
    public bool isGetHit = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject e = collision.gameObject;
        if (e.layer == LayerMask.NameToLayer("Projectile"))
        {
            isGetHit = true;
            mCollidePos = e.transform.localPosition;
        }
    }

    public Vector3 GetCollide()
    {
        isGetHit = false;
        return mCollidePos;
    }
}
