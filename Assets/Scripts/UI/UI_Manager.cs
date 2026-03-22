using System;
using System.Collections.Generic;
using UnityEngine;
using UI.Base;


public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;
    [SerializeField] private List<BaseMenu> _menus = new List<BaseMenu>();
    private Dictionary<MenuType, BaseMenu> _menuDictionary = new Dictionary<MenuType, BaseMenu>();
    
    private MenuType _currentMenu = MenuType.MainMenu;
    private MenuType _lastMenu = MenuType.MainMenu;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(this);
        
        
        InputHandler.OnEscapePress += PlayerPressedEscape;
    }
    
    private void Start()
    {
        InitDictionary();
        SetMenu(MenuType.MainMenu);
    }
    
    private void PlayerPressedEscape()
    {
        if (_menuDictionary.TryGetValue(_currentMenu, out BaseMenu menu))
        {
            menu.EscapePressed();
        }
    }
    
    private void SetMenu(MenuType menuType)
    {
        if (_menuDictionary.TryGetValue(menuType, out BaseMenu menu))
        {
            menu.ShowMenu();
            _currentMenu = menuType;
        }
    }
    
    public void SwapMenu(MenuType menuType)
    {
        if (_menuDictionary.TryGetValue(menuType, out BaseMenu menuToShow)
            && _menuDictionary.TryGetValue(_currentMenu, out BaseMenu menuToHide))
        {
            _lastMenu = _currentMenu;
            _currentMenu = menuType;
            menuToShow.ShowMenu();
            menuToHide.HideMenu();
            Debug.Log(_currentMenu);
        }
    }

    private void InitDictionary()
    {
        foreach (BaseMenu menu in _menus)
        {
            _menuDictionary.TryAdd(menu.ThisMenuType, menu);
            menu.HideMenu();
        }
    }

    public void ReturnToPreviousMenu()
    {
        SwapMenu(_lastMenu);
    }
    
    public MenuType LastMenuType => _lastMenu;
    public MenuType CurrentMenuType => _currentMenu;
}




public enum MenuType
{
    MainMenu,
    CharacterSelectionMenu,
    PauseMenu,
    SongSelectionMenu,
    DifficultySelectionMenu,
    InGameMenu,
    NewHighScore
}