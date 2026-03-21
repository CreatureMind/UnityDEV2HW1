using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BathroomEntrence : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        UI_Manager.Instance.SwapMenu(MenuType.MainMenu);
    }
}
