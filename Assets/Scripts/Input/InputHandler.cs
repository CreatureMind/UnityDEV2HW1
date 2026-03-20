using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static event Action<Direction> OnPress;
    public static event Action OnEscapePress;
    
    public void OnPressLeft(InputAction.CallbackContext context)
    {
        if (context.started)
            OnPress?.Invoke(Direction.Left);
    }
        
    public void OnPressDown(InputAction.CallbackContext context)
    {
        if (context.started)
            OnPress?.Invoke(Direction.Down);
    }
        
    public void OnPressUp(InputAction.CallbackContext context)
    {
        if (context.started)
            OnPress?.Invoke(Direction.Up);
    }
        
    public void OnPressRight(InputAction.CallbackContext context)
    {
        if (context.started)
            OnPress?.Invoke(Direction.Right);
    }
    
    public void OnPressEscape(InputAction.CallbackContext context)
    {
        if (context.started)
            OnEscapePress?.Invoke();
    }
}
