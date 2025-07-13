using UnityEngine;
using TMPro;

[System.Serializable]
public class TimeGradeThresholds
{
    public float APlus = 10f;    // A+ 时间界限（秒）
    public float A = 15f;        // A 时间界限（秒）
    public float B = 20f;        // B 时间界限（秒）
    // 超过 B 时间界限为 C
}
public class CombatTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text gradeText;
    public TimeGradeThresholds gradeThresholds = new TimeGradeThresholds();
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
            gradeText.text = "Grade:"; // 开始新计时时清空评分
        }
    }

    public void StopTimer()
    {
        if (isCounting)
        {
            isCounting = false;
            totalCombatTime += currentSessionTime; // 累计当前阶段时间

            // 计算并显示评分
            float finalTime = totalCombatTime;
            string grade = CalculateGrade(finalTime);
            gradeText.text = grade;

            currentSessionTime = 0f;
            UpdateTimerDisplay();
        }
    }

    public void PauseAndResetTimer()
    {
        isCounting = false;
        totalCombatTime = 0f;
        currentSessionTime = 0f;
        gradeText.text = "Grade:"; // 重置时清空评分
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        float displayTime = isCounting ? totalCombatTime + currentSessionTime : totalCombatTime;
        int minutes = Mathf.FloorToInt(displayTime / 60f);
        int seconds = Mathf.FloorToInt(displayTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private string CalculateGrade(float time)
    {
        if (time <= gradeThresholds.APlus)
        {
            return "Grade:A+";
        }
        else if (time <= gradeThresholds.A)
        {
            return "Grade:A";
        }
        else if (time <= gradeThresholds.B)
        {
            return "Grade:B";
        }
        else
        {
            return "Grade:C";
        }
    }
    public float GetTotalCombatTime()
    {
        return totalCombatTime + (isCounting ? currentSessionTime : 0f);
    }
}
