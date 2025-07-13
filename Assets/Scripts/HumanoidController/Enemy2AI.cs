using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemyAI : HumanoidController
{
    private enum Status
    {
        Idle, Patrolling
    }

    private Status currentStatus = Status.Idle;
    private List<Vector3> waypoints = new List<Vector3>();
    private float statusTimer = 0f;
    private int currentWaypointIndex = -1;
    private float shootingTimer = 0f; // 声明射击计时器
    private bool isShooting = false;
    private float lastCollisionTime = 0f;
    private bool isFacingRight = true;
    private const float COLLISION_COOLDOWN = 1f; // 碰撞冷却时间，避免频繁切换航点

    [SerializeField] private float patrolRange = 10f;
    [SerializeField] private float shootingDuration = 2f;
    [SerializeField] private float waypointReachThreshold = 0.5f;
    [SerializeField] private int fixedSeed = 0;
    [SerializeField] private bool useFixedSeed = false;

    void Start()
    {
        Init();
        // 设置随机种子
        if (!useFixedSeed)
        {
            // 使用当前时间作为随机种子，确保每次游戏运行结果不同
            Random.InitState((int)System.DateTime.Now.Ticks);
        }
        else
        {
            // 使用固定种子（用于测试，确保结果可重现）
            Random.InitState(fixedSeed);
        }

        InitializeWaypoints();
    }

    void Update()
    {
        Vector3 relativePosition = mSightHandler.mTargetPosition - transform.position;
        mBehaviorHandler.mFacingDirection = relativePosition.normalized;

        // 更新面向方向
        if (relativePosition.x > 0) isFacingRight = true;
        if (relativePosition.x < 0) isFacingRight = false;
        GetComponent<SpriteRenderer>().flipX = !isFacingRight;

        // 状态转换逻辑
        if (mSightHandler.isInSight && currentStatus == Status.Idle)
        {
            mBehaviorHandler.Activate(); 
            currentStatus = Status.Patrolling;
        }

        // 状态行为逻辑
        switch (currentStatus)
        {
            case Status.Idle:
                HandleIdleState();
                break;
            case Status.Patrolling:
                HandlePatrolState();
                break;
        }
    }

    private void InitializeWaypoints()
    {
        // 随机生成四个航点
        for (int i = 0; i < 4; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-patrolRange, patrolRange),
                Random.Range(-patrolRange, patrolRange),
                0
            );
            waypoints.Add(transform.position + randomOffset);
        }

        // 选择第一个航点
        SelectNextWaypoint();
    }

    private void SelectNextWaypoint()
    {
        // 选择与当前航点不同的下一个随机航点
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (i != currentWaypointIndex)
            {
                availableIndices.Add(i);
            }
        }

        if (availableIndices.Count > 0)
        {
            currentWaypointIndex = availableIndices[Random.Range(0, availableIndices.Count)];
            isShooting = false;
        }
    }

    private void HandleIdleState()
    {
        mBehaviorHandler.Idle();
        mBehaviorHandler.mMoveDirection = Vector3.zero;
    }

    private void HandlePatrolState()
    {
        if (currentWaypointIndex < 0 || currentWaypointIndex >= waypoints.Count)
        {
            SelectNextWaypoint();
            return;
        }

        Vector3 targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 directionToWaypoint = targetWaypoint - transform.position;

        if (!isShooting)
        {
            // 移动到航点
            mBehaviorHandler.Move();
            mBehaviorHandler.mMoveDirection = directionToWaypoint.normalized;

            // 检查是否到达航点
            if (directionToWaypoint.magnitude < waypointReachThreshold)
            {
                isShooting = true;
                shootingTimer = Time.time;
                mBehaviorHandler.Idle();
                mBehaviorHandler.mMoveDirection = Vector3.zero;
            }
        }
        else
        {
            Vector3 relaPos = mSightHandler.mTargetPosition - transform.localPosition;
            mBehaviorHandler.mFacingDirection = relaPos;
            // 射击状态
            mBehaviorHandler.Shoot();

            // 射击持续时间结束后选择下一个航点
            if (Time.time - shootingTimer > shootingDuration)
            {
                SelectNextWaypoint();
            }
        }
    }

    // 新增：碰撞检测逻辑
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 检查是否与Wall标签的物体碰撞，并且碰撞冷却时间已过
        if (collision.gameObject.CompareTag("Wall") && Time.time - lastCollisionTime > COLLISION_COOLDOWN)
        {
            Debug.Log("Enemy hit wall - switching waypoint");

            // 记录碰撞时间
            lastCollisionTime = Time.time;

            // 如果处于巡逻状态，切换到下一个航点
            if (currentStatus == Status.Patrolling)
            {
                SelectNextWaypoint();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (waypoints == null || waypoints.Count == 0) return;

        Gizmos.color = Color.yellow;
        foreach (Vector3 waypoint in waypoints)
        {
            Gizmos.DrawSphere(waypoint, 0.5f);
        }

        if (currentWaypointIndex >= 0 && currentWaypointIndex < waypoints.Count)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, waypoints[currentWaypointIndex]);
        }
    }
}