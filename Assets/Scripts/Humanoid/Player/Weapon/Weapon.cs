using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActivate = false;
    public Vector3 mFirePoint;
    public Vector3 mTowards;
    // Start is called before the first frame update
    public float mDamageTimes;
    public GameObject mProjectile = null;
    public float mShootRate;
    public float mShootCoolDown;
    public int mSpeed;
    protected float mShootTimer;
    protected float mCoolDownTimer;
    protected Transform parentObject;
    protected Rigidbody2D parentRigid;

    protected void Start()
    {
        parentObject = GetComponentInParent<Transform>();
        parentRigid = GetComponentInParent<Rigidbody2D>();
    }

    virtual public void Shoot()
    {
        GameObject e = Instantiate(mProjectile);
        AudioController.Instance.PlaySfx(AudioController.Instance.HeroShoot);
        e.transform.localPosition = parentObject.TransformPoint(transform.localPosition);
        float angle = Mathf.Atan2(mTowards.y, mTowards.x) * Mathf.Rad2Deg;
        e.transform.rotation = Quaternion.Euler(0, 0, angle);
        int damage = e.GetComponent<ProjectileBehavior>().mDamage;
        damage = (int)(mDamageTimes * damage);
        e.GetComponent<ProjectileBehavior>().mDamage = damage;
        e.GetComponent<ProjectileBehavior>().mSpeed = mSpeed;    
    }
}
