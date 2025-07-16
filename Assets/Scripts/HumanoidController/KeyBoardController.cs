using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardController : HumanoidController
{
    // Start is called before the first frame update
    // private bool towardsRight = true;
    void Start()
    {
        mBehaviorHandler = GetComponent<HumanoidBehavior>();
        mStatusHandler = GetComponent<HumanoidStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mStatusHandler.mIsDead) return;
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) direction.y += 1f;
        if (Input.GetKey(KeyCode.S)) direction.y -= 1f;
        if (Input.GetKey(KeyCode.D)) direction.x += 1f;
        if (Input.GetKey(KeyCode.A)) direction.x -= 1f;
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mBehaviorHandler.mMoveDirection = direction;
        Vector3 facing = mBehaviorHandler.mFacingDirection = mousepos - transform.localPosition;

        if (direction != Vector3.zero)
        {
            mBehaviorHandler.Move();
            if (direction.x > 0) towardsRight = true;
            if (direction.x < 0) towardsRight = false;
        }
        else
        {
            mBehaviorHandler.Idle();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mBehaviorHandler.Dash();
        }

        if (Input.GetMouseButton(0))
        {
            mBehaviorHandler.Shoot();
            if (facing.x > 0) towardsRight = true;
            if (facing.x < 0) towardsRight = false;

        }

        FlipX();
    }
}
