using System;
using UnityEngine;

namespace UI.Base
{
    public abstract class BaseMenu : MonoBehaviour
    {
        [SerializeField] protected MenuType thisMenuType;
        [SerializeField] protected CanvasGroup canvasGroup;

        public static Action<MenuType> OnMenuOpened;
    
        public abstract void ShowMenu();
        public abstract void HideMenu();
        public abstract void EscapePressed();
    
        public MenuType ThisMenuType => thisMenuType;
    }
}