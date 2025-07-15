using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject mainMenuPanel;

    public Slider bgmSlider;
    public Slider sfxSlider;

    void Start()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        bgmSlider.onValueChanged.AddListener(BGMManager.Instance.SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(AudioController.Instance.SetSFXVolume);
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}