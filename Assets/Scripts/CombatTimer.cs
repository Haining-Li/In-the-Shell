using UnityEngine;
using TMPro;

public class CombatTimer : MonoBehaviour
{
    public TMP_Text timerText;
    private float totalCombatTime = 0f; // 累计总战斗时间
    private float currentSessionTime = 0f; // 当前战斗阶段时间
    private bool isCounting = false;
    
    void Update()
    {
        if (isCounting)
        {
            currentSessionTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }
    
    public void StartTimer()
    {
        if (!isCounting)
        {
            isCounting = true;
        }
    }
    
    public void StopTimer()
    {
        if (isCounting)
        {
            isCounting = false;
            totalCombatTime += currentSessionTime; // 累计当前阶段时间
            currentSessionTime = 0f;
            UpdateTimerDisplay();
        }
    }
    
    public void PauseAndResetTimer()
    {
        isCounting = false;
        totalCombatTime = 0f;
        currentSessionTime = 0f;
        UpdateTimerDisplay();
    }
    
    private void UpdateTimerDisplay()
    {
        float displayTime = isCounting ? totalCombatTime + currentSessionTime : totalCombatTime;
        int minutes = Mathf.FloorToInt(displayTime / 60f);
        int seconds = Mathf.FloorToInt(displayTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    public float GetTotalCombatTime()
    {
        return totalCombatTime + (isCounting ? currentSessionTime : 0f);
    }
}
