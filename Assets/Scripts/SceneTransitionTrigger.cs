using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    [Tooltip("要跳转到的场景名称")]
    public string targetSceneName;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查是否是玩家触发了碰撞器
        if (other.CompareTag("Player"))
        {
            // 加载目标场景
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
            
        }
    }
}