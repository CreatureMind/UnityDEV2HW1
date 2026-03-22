using System;
using UnityEngine;

namespace UI.Base
{
    public abstract class BaseMenu : MonoBehaviour
    {
        [SerializeField] protected MenuType thisMenuType;
        [SerializeField] protected CanvasGroup canvasGroup;
        
        
        public abstract void ShowMenu();
        public abstract void HideMenu();
        public abstract void EscapePressed();
        public abstract void ForceStop();
    
        public MenuType ThisMenuType => thisMenuType;
    }
}