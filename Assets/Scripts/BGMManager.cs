using UnityEngine;
using UnityEngine.Audio;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;
    public AudioSource bgmSource;

    void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        LoadVolumeSettings();
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
        PlayerPrefs.Save();
    }

    public void LoadVolumeSettings()
    {
        float volume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        bgmSource.volume = volume;
    }
}

