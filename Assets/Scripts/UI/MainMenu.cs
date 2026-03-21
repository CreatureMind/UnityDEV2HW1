using UI.Base;
using UnityEngine;

public class MainMenu : BaseMenu
{
    public override void ShowMenu()
    {
        
    }

    public override void HideMenu()
    {
    }

    public override void EscapePressed()
    {
        UI_Manager.Instance.SwapMenu(MenuType.PauseMenu);
    }
}
