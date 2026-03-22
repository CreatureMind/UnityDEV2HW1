using System;
using UI.Base;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BathroomEntrence : BaseMenu, IPointerClickHandler
{
    public void Awake()
    {
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
        SceneManager.LoadScene(0);
    }
}
