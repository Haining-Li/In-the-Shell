using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public Image healthBarImage;
    public Sprite[] healthBarSprites;

    private HeroStatus heroStatus;
    private int maxHealth = 100;
    private int lastHealthIndex = -1;

    void Start()
    {
        InitializeHealthSystem();
    }

    void InitializeHealthSystem()
    {
        GameObject hero = GameObject.FindGameObjectWithTag("Player");
        if (hero != null) heroStatus = hero.GetComponent<HeroStatus>();

        if (heroStatus == null)
        {
            Debug.LogError("HeroStatus component not found!");
            return;
        }

        maxHealth = 100;
        UpdateHealthBar();
    }

    void Update()
    {
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