using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
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
        Time.timeScale = 1;
        parent.SetActive(false);
    }

    public void OnGameOver()
    {
        Time.timeScale = 0;
        parent.SetActive(true);
    }
    
    public void OnGiveUpPressed()
    {
        Application.Quit();
    }
}
