using System;
using UnityEngine;

namespace UI.Base
{
    public abstract class BaseMenu : MonoBehaviour
    {
        [SerializeField] private MenuType thisMenuType;

        public static Action<MenuType> OnMenuOpened;
    
        public abstract void ShowMenu();
        public abstract void HideMenu();
        public abstract void EscapePressed();
    
        public MenuType ThisMenuType => thisMenuType;
    }
}