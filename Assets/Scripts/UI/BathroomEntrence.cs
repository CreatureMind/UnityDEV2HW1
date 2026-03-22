using System;
using UI.Base;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BathroomEntrence : BaseMenu, IPointerClickHandler
{
    public static BathroomEntrence Instance;
    private const string SceneName = "MainScene";
    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UI_Manager.Instance.SwapMenu(MenuType.CharacterSelectionMenu);
    }

    public override void ShowMenu()
    {
        SceneManager.LoadScene(1);
    }

    public override void HideMenu()
    {
        EscapePressed();
    }

    public override void EscapePressed()
    {
        if (SceneManager.GetActiveScene().name != SceneName) // replace with your scene name
        {
            SceneManager.LoadScene(0);
        }    
    }

    public override void ForceStop()
    {
        
    }
}
