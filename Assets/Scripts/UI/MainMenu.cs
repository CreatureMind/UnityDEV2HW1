using UI.Base;
using UnityEngine;

public class MainMenu : BaseMenu
{
    public override void ShowMenu()
    {
        UI_Manager.Instance.ForceStopMenu(MenuType.InGameMenu);
    }

    public override void HideMenu()
    {
    }

    public override void EscapePressed()
    {
        UI_Manager.Instance.SwapMenu(MenuType.PauseMenu);
    }

    public override void ForceStop()
    {
        
    }
}
