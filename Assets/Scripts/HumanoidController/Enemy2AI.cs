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
    private float shootingTimer = 0f; // ���������ʱ��
    private bool isShooting = false;
    private float lastCollisionTime = 0f;
    private bool isFacingRight = true;
    private const float COLLISION_COOLDOWN = 1f; // ��ײ��ȴʱ�䣬����Ƶ���л�����

    [SerializeField] private float patrolRange = 10f;
    [SerializeField] private float shootingDuration = 2f;
    [SerializeField] private float waypointReachThreshold = 0.5f;
    [SerializeField] private int fixedSeed = 0;
    [SerializeField] private bool useFixedSeed = false;
    [SerializeField] private Vector2[] WayPoints = null;

    void Start()
    {
        Init();
        // �����������
        if (!useFixedSeed)
        {
            // ʹ�õ�ǰʱ����Ϊ������ӣ�ȷ��ÿ����Ϸ���н����ͬ
            Random.InitState((int)System.DateTime.Now.Ticks);
        }
        else
        {
            // ʹ�ù̶����ӣ����ڲ��ԣ�ȷ����������֣�
            Random.InitState(fixedSeed);
        }

        InitializeWaypoints();
    }

    void Update()
    {
        Vector3 relativePosition = mSightHandler.mTargetPosition - transform.position;
        mBehaviorHandler.mFacingDirection = relativePosition.normalized;

        // ����������
        if (relativePosition.x > 0) isFacingRight = true;
        if (relativePosition.x < 0) isFacingRight = false;
        GetComponent<SpriteRenderer>().flipX = !isFacingRight;

        // ״̬ת���߼�
        if (mSightHandler.isInSight && currentStatus == Status.Idle)
        {
            mBehaviorHandler.Activate(); 
            currentStatus = Status.Patrolling;
        }

        // ״̬��Ϊ�߼�
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
        // ��������ĸ�����
        for (int i = 0; i < 4; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-patrolRange, patrolRange),
                Random.Range(-patrolRange, patrolRange),
                0
            );
            waypoints.Add(transform.position + randomOffset);
        }

        // ѡ���һ������
        SelectNextWaypoint();
    }

    private void SelectNextWaypoint()
    {
        // ѡ���뵱ǰ���㲻ͬ����һ���������
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
            // �ƶ�������
            mBehaviorHandler.Move();
            mBehaviorHandler.mMoveDirection = directionToWaypoint.normalized;

            // ����Ƿ񵽴ﺽ��
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
            // ���״̬
            mBehaviorHandler.Shoot();

            // �������ʱ�������ѡ����һ������
            if (Time.time - shootingTimer > shootingDuration)
            {
                SelectNextWaypoint();
            }
        }
    }

    // ��������ײ����߼�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ����Ƿ���Wall��ǩ��������ײ��������ײ��ȴʱ���ѹ�
        if (collision.gameObject.CompareTag("Wall") && Time.time - lastCollisionTime > COLLISION_COOLDOWN)
        {
            Debug.Log("Enemy hit wall - switching waypoint");

            // ��¼��ײʱ��
            lastCollisionTime = Time.time;

            // �������Ѳ��״̬���л�����һ������
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