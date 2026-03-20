using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private List<BaseMenu> _menus = new List<BaseMenu>();
    private Dictionary<MenuType, BaseMenu> _menuDictionary = new Dictionary<MenuType, BaseMenu>();
    
    private MenuType _currentMenu;
    private MenuType _lastMenu;

    private void Start()
    {
        InitDictionary();
        InputHandler.OnEscapePress += PlayerPressedEscape;
        BaseMenu.OnMenuOpened += SwapMenu;
    }
    
    private void PlayerPressedEscape()
    {
        switch (_currentMenu)
        {
            case MenuType.BarMenu:
                SwapMenu(MenuType.MainMenu);
                break;
            case MenuType.SongSelectionMenu:
                SwapMenu(MenuType.BarMenu);
                break;
            case MenuType.DifficultySelectionMenu:
                SwapMenu(MenuType.SongSelectionMenu);
                break;
            case MenuType.PauseMenu:
                SwapMenu(_lastMenu);
                break;
            case MenuType.InGameMenu:
                SwapMenu(MenuType.PauseMenu);
                break;
            case MenuType.MainMenu:
                break;
        }
    }
    
    private void SwapMenu(MenuType menuType)
    {
        if (_currentMenu == menuType)
            return;
        
        if (_menuDictionary.TryGetValue(menuType, out BaseMenu menuToShow)
            && _menuDictionary.TryGetValue(_currentMenu, out BaseMenu menuToHide))
        {
            menuToShow.ShowMenu();
            menuToHide.HideMenu();
        }

        _lastMenu = _currentMenu;
        _currentMenu = menuType;
    }

    private void InitDictionary()
    {
        foreach (BaseMenu menu in _menus)
        {
            _menuDictionary.TryAdd(menu.ThisMenuType, menu);
        }
    }
}


public abstract class BaseMenu : MonoBehaviour
{
    [SerializeField] private MenuType thisMenuType;

    public static Action<MenuType> OnMenuOpened;
    
    public abstract void ShowMenu();
    public abstract void HideMenu();
    
    public MenuType ThisMenuType => thisMenuType;
}

public enum MenuType
{
    MainMenu,
    BarMenu,
    PauseMenu,
    SongSelectionMenu,
    DifficultySelectionMenu,
    InGameMenu,
}