using UnityEngine;
using UnityEngine.UI;

public class Enemy3BloodBar : MonoBehaviour
{
    public Image healthBarImage;
    public Sprite[] healthBarSprites;

    private Enemy3Status enemyStatus;
    private int maxHealth;
    private int lastHealthIndex = -1;

    void Start()
    {
        InitializeHealthSystem();
    }

    void InitializeHealthSystem()
    {
        enemyStatus = GetComponentInParent<Enemy3Status>();

        if (enemyStatus == null)
        {
            Debug.LogError("enemyStatus component not found!");
            return;
        }

        maxHealth = enemyStatus.mMaxHealthPoint;
        UpdateHealthBar();
    }

    void Update()
    {
        if (enemyStatus != null)
        {
            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        int currentIndex = (healthBarSprites.Length - 2) * enemyStatus.mHealthPoint / maxHealth + 1;
        if (enemyStatus.mHealthPoint == 0) currentIndex = 0;
        healthBarImage.sprite = healthBarSprites[currentIndex];
        lastHealthIndex = currentIndex;
    }
}