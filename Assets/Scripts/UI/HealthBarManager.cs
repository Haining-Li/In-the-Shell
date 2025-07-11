using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    // UI组件和精灵引用
    public Image healthBarImage;
    public Sprite[] healthBarSprites; // 在Inspector中按顺序拖入27张图片

    // 血量系统绑定
    private HeroStatus heroStatus;
    private int maxHealth = 100;
    private int lastHealthIndex = -1;

    void Start()
    {
        InitializeHealthSystem();
    }

    void InitializeHealthSystem()
    {
        // 动态查找HeroStatus组件（适应prefab实例化）
        GameObject hero = GameObject.FindGameObjectWithTag("Player");
        if (hero != null) heroStatus = hero.GetComponent<HeroStatus>();

        if (heroStatus == null)
        {
            Debug.LogError("HeroStatus component not found!");
            return;
        }

        maxHealth = 100; // 最大血量为100
        UpdateHealthBar();
    }

    void Update()
    {
        // 每帧检查血量变化
        if (heroStatus != null)
        {
            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
            int currentIndex = 25 * heroStatus.mHealthPoint / maxHealth + 1;
            if(heroStatus.mHealthPoint == 0) currentIndex = 0; 
            healthBarImage.sprite = healthBarSprites[currentIndex];
            lastHealthIndex = currentIndex;
    }
}