using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenButton : MonoBehaviour
{
    PlayerInput inputAction;

    public void Awake()
    {
        inputAction = new PlayerInput();
        inputAction.PlayerControls.Jump.performed += (ctx) => StartLoad();
    }

    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }

    public void StartLoad()
    {
        SceneManager.LoadScene(1);
    }
}
