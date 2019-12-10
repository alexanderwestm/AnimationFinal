using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    Vector2 lookAxis;
    PlayerInput inputAction;
    private void Awake()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;

        inputAction = new PlayerInput();
        //inputAction.PlayerControls.Movement.performed += (ctx) => moveVector = ctx.ReadValue<Vector2>();
        //inputAction.PlayerControls.Jump.performed += (ctx) => InterpretKeyboardInput('_', KeyState.DOWN);
        inputAction.PlayerControls.Look.performed += InterpretMouseMoveEvent;

        //inputAction.PlayerControls.Dash.performed += (ctx) => InterpretKeyboardInput('h', KeyState.DOWN);
        //inputAction.PlayerControls.PrimaryAttack.performed += (ctx) => InterpretMouseInput('0', KeyState.DOWN);
        //inputAction.PlayerControls.SecondaryAttack.performed += (ctx) => InterpretMouseInput('1', KeyState.DOWN);
        //inputAction.PlayerControls.PrimaryAttack.canceled += (ctx) => InterpretMouseInput('0', KeyState.UP);
        //inputAction.PlayerControls.SecondaryAttack.canceled += (ctx) => InterpretMouseInput('1', KeyState.UP);
        //
        //inputAction.PlayerControls.SwapLoadout.performed += (ctx) => InterpretKeyboardInput('Q', KeyState.DOWN);
    }

    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }

    public void InterpretMouseMoveEvent(InputAction.CallbackContext ctx)
    {
        lookAxis = ctx.ReadValue<Vector2>();
    }

    public float GetAxisCustom(string axisName)
    {
        if (axisName == "VerticalCam")
        {
            return lookAxis.y;
        }
        else if (axisName == "HorizontalCam")
        {
            return lookAxis.x;
        }
        return 0;
    }
}
