using UnityEngine;
using UnityEngine.UI;

public class ComputerInteraction : MonoBehaviour
{
    public GameObject textDisplayPanel; // 在Unity编辑器中分配的UI面板
    public Text displayText; // 在Unity编辑器中分配的Text组件
    
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Sprite blackScreenSprite;
    private bool isInteracting = false;
    private bool playerInRange = false;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        blackScreenSprite = spriteRenderer.sprite;
        
        // 初始状态：黑屏，动画暂停
        animator.enabled = false;
        textDisplayPanel.SetActive(false);
    }
    
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleInteraction();
        }
        
        // 如果玩家不在范围内但交互界面仍显示，则关闭界面
        if (!playerInRange && isInteracting)
        {
            EndInteraction();
        }
    }
    
    private void ToggleInteraction()
    {
        isInteracting = !isInteracting;
        
        if (isInteracting)
        {
            // 开始交互：显示文本面板
            textDisplayPanel.SetActive(true);
            animator.enabled = false;
            spriteRenderer.sprite = blackScreenSprite;
        }
        else
        {
            // 结束交互
            EndInteraction();
        }
    }
    
    private void EndInteraction()
    {
        isInteracting = false;
        textDisplayPanel.SetActive(false);
        animator.enabled = true;
    }
    
    // 碰撞体进入检测
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    
    // 碰撞体离开检测
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
