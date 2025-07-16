using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    [Header("References")]
    public WeaponSelectionManager weaponSelectionManager;
    private bool isPlayerInRange = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("按下E");
            weaponSelectionManager.OpenSelectionPanel();
        }
    }
    
    // 碰撞体进入检测
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }
    
    // 碰撞体离开检测
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // 离开时关闭选择界面
            weaponSelectionManager.CloseSelectionPanel();
        }
    }
}