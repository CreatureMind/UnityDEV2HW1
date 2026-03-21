using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CinemachineCameraController : MonoBehaviour
{
    private CinemachineFreeLook freeLook;

    void Awake()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
        
        if (freeLook == null)
        {
            Debug.LogError("CinemachineFreeLook component not found!");
            return;
        }
        
        if (freeLook.Follow == null || freeLook.LookAt == null)
        {
            Debug.LogError("CinemachineFreeLook needs valid Follow and LookAt targets!");
        }
    }

    void Update()
    {
        if (freeLook == null) return;
        
        if (Mouse.current.rightButton.isPressed)
        {
            freeLook.m_XAxis.m_InputAxisName = "Mouse X";
            freeLook.m_YAxis.m_InputAxisName = "Mouse Y";

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            freeLook.m_XAxis.m_InputAxisName = "";
            freeLook.m_YAxis.m_InputAxisName = "";
            freeLook.m_XAxis.m_InputAxisValue = 0;
            freeLook.m_YAxis.m_InputAxisValue = 0;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}