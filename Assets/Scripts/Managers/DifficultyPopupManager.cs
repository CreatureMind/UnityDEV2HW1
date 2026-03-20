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
        ArrowsSpawner.OnSongEnded += GameOver;
        HpBar.OnHpEnded += GameOver;
    }

    void OnDisable()
    {
        ArrowsSpawner.OnSongEnded -= GameOver;
    }

    public void OnSelectedPressed(DdrPattern pattern)
    {
        OnSelected?.Invoke(pattern);

        Time.timeScale = 1;
        parent.SetActive(false);
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        parent.SetActive(true);
    }
    
    public void OnGiveUpPressed()
    {
        Application.Quit();
    }
}
