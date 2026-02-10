using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyPopupManager : MonoBehaviour
{
    [SerializeField] private GameObject parent;

    public static event Action<DdrPattern> OnSelected;

    void OnEnable()
    {
        Time.timeScale = 0;
        ArrowsSpawner.OnSongEnded += OnGameOver;
    }

    public void OnSelectedPressed(DdrPattern pattern)
    {
        OnSelected?.Invoke(pattern);
        SoundManager.instance.PlayMusic(pattern.songName);
        Time.timeScale = 1;
        parent.SetActive(false);
    }

    private void OnGameOver(DdrPattern pattern)
    {
        Time.timeScale = 0;
        SoundManager.instance.StopMusic(pattern.songName);
        parent.SetActive(true);
    }
    
    public void OnGiveUpPressed()
    {
        Application.Quit();
    }
}
