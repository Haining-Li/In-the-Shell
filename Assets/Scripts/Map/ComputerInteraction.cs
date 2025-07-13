using UnityEngine;
using UnityEngine.UI;

public class ComputerInteraction : MonoBehaviour
{
    public float interactionDistance = 2f;
    public GameObject textDisplayPanel; // 在Unity编辑器中分配的UI面板
    public Text displayText; // 在Unity编辑器中分配的Text组件
    
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Sprite blackScreenSprite;
    private bool isInteracting = false;
    private Transform heroTransform;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        blackScreenSprite = spriteRenderer.sprite;
        heroTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        // 初始状态：黑屏，动画暂停
        animator.enabled = false;
        textDisplayPanel.SetActive(false);
    }
    
    void Update()
    {

        // 检查heroTransform是否有效
        if (heroTransform == null)
        {
            // 尝试重新获取玩家引用
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                heroTransform = player.transform;
            }
            else
            {
                // 如果找不到玩家，直接返回
                return;
            }
        }
        if (Vector2.Distance(transform.position, heroTransform.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isInteracting)
                {
                    // 开始交互：显示文本面板
                    isInteracting = true;
                    textDisplayPanel.SetActive(true);
                    animator.enabled = false;
                    spriteRenderer.sprite = blackScreenSprite;
                }
                else
                {
                    // 结束交互：隐藏文本面板，恢复动画
                    isInteracting = false;
                    textDisplayPanel.SetActive(false);
                    animator.enabled = true;
                }
            }
        }
    }
}