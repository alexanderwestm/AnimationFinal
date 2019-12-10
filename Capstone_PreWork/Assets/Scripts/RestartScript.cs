using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    PlayerInput input;
    bool startHit = false;
    bool selectHit = false;

    // Start is called before the first frame update
    void Awake()
    {
        input = new PlayerInput();

        input.PlayerControls.StartButton.performed += (ctx) => startHit = true;
        input.PlayerControls.SelectButton.performed += (ctx) => selectHit = true;

        input.PlayerControls.StartButton.canceled += (ctx) => startHit = false;
        input.PlayerControls.SelectButton.canceled += (ctx) => selectHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(startHit && selectHit)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
