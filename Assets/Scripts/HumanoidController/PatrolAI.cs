using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PatrolAI : HumanoidController
{
    [SerializeField] Vector2[] WayPoints = null;
    #if UNITY_EDITOR
    
    void OnDrawGizmosSelected()
    {
        if (WayPoints == null ||WayPoints .Length == 0) return;

        // 绘制路径线
        Handles.color = Color.yellow;
        for (int i = 0; i < WayPoints.Length - 1; i++)
        {
            Vector3 start = WayPoints[i];
            Vector3 end = WayPoints[i + 1];
            Handles.DrawDottedLine(start, end, 5f);
        }

        // 绘制可拖拽的点
        Handles.color = Color.red;
        for (int i = 0; i < WayPoints.Length; i++)
        {
            Vector3 worldPos = WayPoints[i];
            float size = HandleUtility.GetHandleSize(worldPos) * 0.1f;
            
            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.FreeMoveHandle(
                GUIUtility.GetControlID(FocusType.Passive),
                worldPos,
                size,
                Vector3.zero,
                Handles.SphereHandleCap
            );
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this, "Move Waypoint");
                WayPoints[i] = transform.InverseTransformPoint(newPos);
            }

            // 显示序号标签
            Handles.Label(worldPos, $"Point {i}", new GUIStyle { normal = { textColor = Color.white } });
        }
    }
    #endif

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivated) return;

        Patrol();
        if (mSightHandler.isInSight)
        {
            Attack();
        }
    }

    private int next = 0;
    private float range = 10f;
    void Patrol()
    {
        int len = WayPoints.Length;
        Vector3 nextWayPoint = WayPoints[next];
        if ((nextWayPoint - transform.localPosition).magnitude < len)
        {
            next = (next + 1) % len;
        }
        else
        {
            mBehaviorHandler.mMoveDirection = nextWayPoint - transform.localPosition;
            mBehaviorHandler.Move();
        }
    }

    [SerializeField] private float ShootDuration = 1f;
    [SerializeField] private float CoolDown = 2f;
    private bool isCoolDown = true;

    void Attack()
    {
        if (isCoolDown)
        {
            if (Time.time - mStatusTimer < ShootDuration)
            {
                Vector3 relaPos = mSightHandler.mTargetPosition - transform.localPosition;
                mBehaviorHandler.mFacingDirection = relaPos;
                mBehaviorHandler.Shoot();
            }
            else
            {
                isCoolDown = false;
                mStatusTimer = Time.time;
            }
        }
        else
        {
            if (Time.time - mStatusTimer > CoolDown)
            {
                isCoolDown = true;
                mStatusTimer = Time.time;
            }
        }
    }
}

