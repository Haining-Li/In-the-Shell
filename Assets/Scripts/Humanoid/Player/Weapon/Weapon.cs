using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActivate = false;
    public Vector3 mFirePoint;
    public Vector3 mTowards;
    // Start is called before the first frame update
    public GameObject mProjectile = null;
    public float mShootRate;
    public float mShootCoolDown;
    protected float mShootTimer;
    protected float mCoolDownTimer;
    protected Transform parentObject;

    protected void Start()
    {
        parentObject = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    virtual public void Shoot()
    {
        GameObject e = Instantiate(mProjectile);

        e.transform.localPosition = parentObject.TransformPoint(transform.localPosition);
        float angle = Mathf.Atan2(mTowards.y, mTowards.x) * Mathf.Rad2Deg;
        e.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
