using UnityEngine;
using System.Collections;

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
        if(inRange && Input.GetKeyDown(KeyCode.E) && !isOpen)
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
    }
}