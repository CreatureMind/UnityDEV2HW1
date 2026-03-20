using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CinemachineCameraController : MonoBehaviour
{
    private CinemachineFreeLook freeLook;

    void Awake()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        
            // New Input System check for Right Mouse Button
            if (Mouse.current.rightButton.isPressed)
            {
                freeLook.m_XAxis.m_InputAxisName = "Mouse X";
                freeLook.m_YAxis.m_InputAxisName = "Mouse Y";

                // Hide cursor
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                freeLook.m_XAxis.m_InputAxisName = "";
                freeLook.m_YAxis.m_InputAxisName = "";
                freeLook.m_XAxis.m_InputAxisValue = 0;
                freeLook.m_YAxis.m_InputAxisValue = 0;

                // Show cursor
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }