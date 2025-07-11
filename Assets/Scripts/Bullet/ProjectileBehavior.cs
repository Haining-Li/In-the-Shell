using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bullet Behavior
// Two status: Flying and Hit
// Three behavior: Move Forward, Hit something, Destroyed

public class ProjectileBehavior : MonoBehaviour
{
    // Status
    protected Animator mAnimator = null;
    public enum ProjectileStatus
    {
        Flying, Crash, Destroyed
    };

    // information about Bullet should include its speed
    public float mSpeed = 10f;
    protected float mFlyingDuration = 4f;
    protected float mHitDuration = 0.2f;
    protected ProjectileStatus mStatus = ProjectileStatus.Flying;
    protected float mStatusTimer = 0f;
    public int mDamage = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Init()
    {
        mAnimator = GetComponent<Animator>();
    }

    // Moving forward at a constant speed
    virtual public void Move()
    {
        Vector3 pos = transform.localPosition;
        pos += mSpeed * Time.smoothDeltaTime * transform.right;
        transform.localPosition = pos;
        
        
    }

    // Hit something, the wall or humanoid
    virtual public void Hit()
    {

    }

    // Destroyed it self
    virtual public void Destroy()
    {
        Destroy(transform.gameObject);
    }

    [Header("AllowCollide")]
    public bool Hero = false;
    public bool Enemy = false;


}
