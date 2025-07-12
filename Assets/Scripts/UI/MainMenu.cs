using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelSelectionPanel;
    [SerializeField] private GameObject creditPanel;
    
    public void ShowLevelSelection()
    {
        mainMenuPanel.SetActive(false);
        levelSelectionPanel.SetActive(true);
    }

    public void ShowCredit()
    {
        mainMenuPanel.SetActive(false);
        creditPanel.SetActive(true);
    }
    
    public void BackToMainMenu()
    {
        levelSelectionPanel.SetActive(false);
        creditPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    
    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
