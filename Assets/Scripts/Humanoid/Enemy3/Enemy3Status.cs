using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Status : HumanoidStatus
{
    // Start is called before the first frame update
    [SerializeField] private GameObject bloodPackagePrefab;
    AudioController audioController;
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        audioController = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mIsDying)
        {
            AnimatorStateInfo stateInfo = mAnimator.GetCurrentAnimatorStateInfo(0);
            if (Time.time - mStatusTimer > stateInfo.length)
            {
                Die();
            }
            else
            {
            }
        }
    }

    public override void GetHurt(int damage)
    {
        // Debug.Log(mHP);
        base.GetHurt(damage);
        if (mHealthPoint == 0 && !mIsDying)
        {
            mIsDying = true;
            mAnimator.SetBool("Die", true);
            mStatusTimer = Time.time;
        }
        else
        {
            mAnimator.SetTrigger("GetHurt");
        }
    }

    public override void Die()
    {
        audioController.PlaySfx(audioController.Enemy3Death);
        base.Die();
        if (Random.Range(0f, 1f) <= 1f)
        {
            Instantiate(bloodPackagePrefab, transform.position, Quaternion.identity);
        }
    }
}
