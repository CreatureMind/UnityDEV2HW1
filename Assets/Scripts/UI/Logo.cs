using Cinemachine;
using UnityEngine;

public class Logo : MonoBehaviour
{
    [Header("Cinemachine")]
    [SerializeField] private CinemachineBrain brain;
    [SerializeField] private CinemachineVirtualCamera cam;
    
    private bool _clicked = false;
    public void OnClick()
    {
        UI_Manager.Instance.SwapMenu(MenuType.MainMenu);
        cam.Priority = 0;
        _clicked = true;
    }
    
    private void Update()
    {
        if (!_clicked) return;
        
        if (!brain.IsBlending && brain.ActiveVirtualCamera != cam)
        {
            Destroy(gameObject);
        }
    }
}
