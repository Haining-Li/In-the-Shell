using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;
    public AudioSource bgmSource;
    public AudioClip[] bgmClips = new AudioClip[3];

    private int currentBGMIndex = 0; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadVolumeSettings();
        SceneManager.sceneLoaded += OnSceneLoaded; // 注册场景加载事件
        PlayCurrentBGM(); 
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayNextBGM();
    }

    public void PlayCurrentBGM()
    {
        if (bgmClips.Length == 0 || currentBGMIndex >= bgmClips.Length)
        {
            Debug.LogWarning("BGM clips not assigned or index out of range!");
            return;
        }

        bgmSource.clip = bgmClips[currentBGMIndex];
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayNextBGM()
    {
        currentBGMIndex = (currentBGMIndex + 1) % bgmClips.Length;
        PlayCurrentBGM();
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

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}