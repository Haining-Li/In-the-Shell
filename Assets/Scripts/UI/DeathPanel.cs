using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPanel : MonoBehaviour
{
    public GameObject deathPanel;

    // Start is called before the first frame update
    void Start()
    {
        deathPanel = gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Respawn()
    {
        Time.timeScale = 1f;
        deathPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToLevelMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
