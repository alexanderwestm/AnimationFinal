using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum KeyState
{
    NULL_STATE = 0,
    DOWN,
    UP,
    HELD
}
public class MouseEvent : Event
{
    private int buttonNum;
    private Vector2 screenPos;
    private KeyState buttonState;
    public MouseEvent(int eventPriority, int button, Vector2 position, KeyState state) : base(EventType.MOUSE_INPUT_EVENT, eventPriority)
    {
        buttonNum = button;
        screenPos = position;
        buttonState = state;
       
    }

    public int GetButtonNum()
    {
        return buttonNum;
    }

    public Vector2 GetScreenPos()
    {
        return screenPos;
    }

    public bool GetIsUp()
    {
        return buttonState == KeyState.UP;
    }

    public KeyState GetState()
    {
        return buttonState;
    }

}

public class MouseMoveEvent : Event
{
    public Vector2 mouseAxis { get; private set; }
    public MouseMoveEvent(int eventPriority, Vector2 axis) : base(EventType.MOUSE_MOVE_EVENT, eventPriority)
    {
        mouseAxis = axis;
    }

    public Vector2 GetAxis()
    {
        return mouseAxis;
    }
}



public class ButtonEvent : Event
{
    private char buttonPressed;

    //0 for down, 1 for up
    private KeyState buttonState;
    public ButtonEvent(int eventPriority, char button, KeyState state) : base(EventType.KEY_INPUT_EVENT, eventPriority)
    {
        buttonPressed = button;
        buttonState = state;
    }

    public char GetButtonName()
    {
        return buttonPressed;
    }

    public bool GetIsUp()
    {
        return buttonState == KeyState.UP;
    }


    public KeyState GetState()
    {
        return buttonState;
    }

}