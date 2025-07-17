using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class DoorController : MonoBehaviour
{
    [Header("Door Sprites")]
    [Tooltip("Closed with red light (default state)")]
    public Sprite closedRedState;
    [Tooltip("Activating with green light")]
    public Sprite activatingState;
    [Tooltip("Opening state 1")]
    public Sprite openingState1;
    [Tooltip("Opening state 2")]
    public Sprite openingState2;
    [Tooltip("Fully open state")]
    public Sprite openState;

    [Header("Animation Settings")]
    public float activationDelay = 0.5f;
    public float openingInterval = 0.3f;

    [Header("Room Type")]
    [Tooltip("是否战斗房间的入口门")]
    public bool isCombatRoomDoor = false;
    [Tooltip("是否剧情房间的入口门")]
    public bool isStoryRoomDoor = false;

    [Header("Enemy Spawn Settings")]
    [Tooltip("Enemy prefabs to spawn when door opens")]
    public List<GameObject> enemyPrefabs;
    [Tooltip("Positions to spawn enemies (world coordinates)")]
    public List<Vector2> spawnPositions;
    [Tooltip("Delay after door opens to spawn enemies")]
    public float spawnDelay = 0.5f;

    private Collider2D doorCollider;
    private SpriteRenderer spriteRenderer;
    private bool isOpen = false;
    private CombatTimer combatTimer; // 新增计时器引用

    public bool IsOpen { get { return isOpen; } }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
        ResetDoor();
    }

    void Start()
    {
        // 获取场景中的计时器
        combatTimer = FindObjectOfType<CombatTimer>();
    }

    public void ResetDoor()
    {
        spriteRenderer.sprite = closedRedState;
        isOpen = false;
    }

    void Update()
    {
        bool inRange = GetComponentInChildren<Detect>().InRange;
        if (inRange && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            StartCoroutine(OpenDoorSequence());
        }
    }

    IEnumerator OpenDoorSequence()
    {
        // 切换到激活状态(绿灯)
        spriteRenderer.sprite = activatingState;
        yield return new WaitForSeconds(activationDelay);

        // 开门动画第一部分
        spriteRenderer.sprite = openingState1;
        yield return new WaitForSeconds(openingInterval);

        // 开门动画第二部分
        spriteRenderer.sprite = openingState2;
        yield return new WaitForSeconds(openingInterval);

        // 完全打开状态
        spriteRenderer.sprite = openState;
        doorCollider.enabled = false;
        isOpen = true;

        // 新增：根据门类型触发计时器逻辑
        if (combatTimer != null)
        {
            if (isCombatRoomDoor)
            {
                combatTimer.StartTimer();
            }
            else if (isStoryRoomDoor)
            {
                combatTimer.StopTimer();
            }
        }

        // 新增：开始刷怪协程
        // StartCoroutine(SpawnEnemies());
        StartCoroutine(ActivateEnemies());
    }

    /*
    IEnumerator SpawnEnemies()
    {
        // 等待指定的延迟时间
        yield return new WaitForSeconds(spawnDelay);

        // 确保敌人预制件和生成点数量匹配
        int spawnCount = Mathf.Min(enemyPrefabs.Count, spawnPositions.Count);

        // 生成敌人
        for (int i = 0; i < spawnCount; i++)
        {
            if (enemyPrefabs[i] != null)
            {
                Instantiate(enemyPrefabs[i], spawnPositions[i], Quaternion.identity);
            }
        }
    }
    */

    [SerializeField] private GameObject[] Enemies = null;
    IEnumerator ActivateEnemies()
    {
        yield return new WaitForSeconds(spawnDelay);

        int num = Enemies.Length;
        for (int i = 0; i < num; i++)
        {
            if (Enemies[i] != null)
            {
                Enemies[i].SetActive(true);
                HumanoidController[] bhs = Enemies[i].GetComponentsInChildren<HumanoidController>();
                for (int j = 0; j < bhs.Length; j++)
                {
                    bhs[j].Activate();
                }
            }
        }
    }
}