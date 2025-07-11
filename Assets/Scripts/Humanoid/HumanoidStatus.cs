using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HumanoidStatus : MonoBehaviour
{
    protected Animator mAnimator = null;
    public int mMaxHealthPoint = 100;
    [SerializeField]
    protected int mHP = 100;
    public int mHealthPoint
    {
        get { return mHP; }
    }
    protected bool mIsDying = false;
    public bool mIsDead
    {
        get { return mIsDying; }
    }
    protected float mStatusTimer = 0f;


    protected void Init()
    {
        mAnimator = GetComponent<Animator>();
        mHP = mMaxHealthPoint;
    }
    virtual public void GetHurt(int damage)
    {
        if (mHealthPoint - damage <= 0)
        {
            mHP = 0;
        }
        else
        {
            mHP -= damage;
        }
    }

    virtual public void Die()
    {
        if (mHealthPoint <= 0)
        {
            Destroy(transform.gameObject);
        }
    }

    virtual public void Recover(int health)
    {
        if (mHealthPoint + health <= mMaxHealthPoint)
        {
            mHP += health;
        }
        else
        {
            mHP = 100;
        }
    }
}
