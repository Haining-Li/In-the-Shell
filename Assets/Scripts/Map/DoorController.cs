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
    
    private Collider2D doorCollider;
    public float activationDelay = 0.5f;
    public float openingInterval = 0.3f;
    
    private SpriteRenderer spriteRenderer;
    private bool isOpen = false;

    public bool IsOpen { get { return isOpen; } }
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
        ResetDoor();
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
        doorCollider.enabled = false; // 禁用碰撞体
        isOpen = true;
    }
    
}