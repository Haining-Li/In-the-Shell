using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraBehavior : MonoBehaviour
{
    public GameObject mFocus = null;
    public float mFollowRate = 0.01f;
    
    void Start()
    {
        Debug.Assert(mFocus != null, "Camera focus target is not assigned!");
    }

    void Update()
    {
        // 添加空引用检查
        if (mFocus == null)
        {
            Debug.LogWarning("Camera focus target is missing!");
            return;
        }
        
        Follow();
    }

    void Follow()
    {
        // 再次检查确保安全
        if (mFocus == null) return;
        
        Vector3 pos = mFocus.transform.localPosition;
        pos = Vector3.LerpUnclamped(transform.localPosition, pos, mFollowRate);
        pos.z = -10;
        transform.localPosition = pos;
    }
}