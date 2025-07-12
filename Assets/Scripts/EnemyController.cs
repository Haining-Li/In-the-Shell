using UnityEngine;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject enemyPrefab;
        public int count;
    }

    [Header("门关联设置")]
    public DoorController linkedDoor; // 关联的门

    [Header("生成区域设置")]
    public Vector2 spawnAreaCenter;
    public Vector2 spawnAreaSize;

    [Header("敌人类型设置")]
    public List<EnemyType> enemyTypes = new List<EnemyType>();

    [Header("生成设置")]
    public float spawnAttemptsPerEnemy = 10; // 每个敌人尝试生成次数
    public float spawnDelay = 0.5f; // 门打开后的延迟时间


    private bool hasSpawned = false;

    void Start()
    {
        int seed = (int)System.DateTime.Now.Ticks;
        Debug.Log(seed);
        Random.InitState(seed);
    }
    void Update()
    {
        // 检查关联的门是否打开且未生成过敌人
        if (!hasSpawned && linkedDoor != null && linkedDoor.IsOpen)
        {
            hasSpawned = true;
            Invoke("SpawnEnemies", spawnDelay);
        }
    }

    public void SpawnEnemies()
    {
        foreach (var enemyType in enemyTypes)
        {
            for (int i = 0; i < enemyType.count; i++)
            {
                SpawnEnemyWithRetry(enemyType.enemyPrefab);
            }
        }
    }

    private void SpawnEnemyWithRetry(GameObject enemyPrefab)
    {
        for (int attempt = 0; attempt < spawnAttemptsPerEnemy; attempt++)
        {
            Vector2 spawnPosition = GetRandomPositionInArea();
            
            if (!IsPositionOccupied(spawnPosition))
            {
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                return;
            }
        }
        
        Debug.LogWarning($"Failed to spawn enemy after {spawnAttemptsPerEnemy} attempts");
    }

    private Vector2 GetRandomPositionInArea()
    {
        float x = Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, 
                              spawnAreaCenter.x + spawnAreaSize.x / 2);
        float y = Random.Range(spawnAreaCenter.y - spawnAreaSize.y / 2, 
                              spawnAreaCenter.y + spawnAreaSize.y / 2);
        return new Vector2(x, y);
    }

    private bool IsPositionOccupied(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.5f);
        return colliders.Length > 0;
    }

    // 在场景视图中绘制生成区域（仅用于调试）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(spawnAreaCenter, spawnAreaSize);
    }
}
