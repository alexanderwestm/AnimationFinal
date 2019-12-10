using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Collections;

public class PlayerInputInterpreter : MonoBehaviour
{
    Vector3 moveVector = Vector3.zero;
    public Vector3 rotatedMove = Vector3.zero;
    Vector2 lookAxis = Vector3.zero;
    CharacterState characterState;
    CharacterSkillSet skillSet;
    CommandLogger logger;
    Animator anim;
    string recentCamLookDevice = "";

    PlayerInput inputAction;

    Rigidbody rb;
    [SerializeField]
    ControlVariables playerControlVariables;

    ParticleSystem jumpUpParticles; 


    bool isPlayer;

    Menu_Open menu;
    bool processInput = true;

    Camera mainCamera;

    bool holdingAbility = false;
    bool lastHolding = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        skillSet = GetComponent<CharacterSkillSet>();
        characterState = GetComponent<CharacterState>();
        logger = GetComponent<CommandLogger>();
        isPlayer = gameObject.tag != "PlayerClone" && gameObject.tag != "Untagged";

        if (isPlayer)
        {
            CinemachineCore.GetInputAxis = GetAxisCustom;
        
            inputAction = new PlayerInput();
            inputAction.PlayerControls.Movement.performed += (ctx) => moveVector = ctx.ReadValue<Vector2>();
            inputAction.PlayerControls.Jump.performed += (ctx) => InterpretKeyboardInput('_', KeyState.DOWN);
            inputAction.PlayerControls.Look.performed += InterpretMouseMoveEvent;

            inputAction.PlayerControls.Dash.performed += (ctx) => InterpretKeyboardInput('h', KeyState.DOWN);
            inputAction.PlayerControls.PrimaryAttack.performed += (ctx) => InterpretMouseInput('0', KeyState.DOWN);
            inputAction.PlayerControls.SecondaryAttack.performed += (ctx) => InterpretMouseInput('1', KeyState.DOWN);
            inputAction.PlayerControls.PrimaryAttack.canceled += (ctx) => InterpretMouseInput('0', KeyState.UP);
            inputAction.PlayerControls.SecondaryAttack.canceled += (ctx) => InterpretMouseInput('1', KeyState.UP);

            inputAction.PlayerControls.SwapLoadout.performed += (ctx) => InterpretKeyboardInput('Q', KeyState.DOWN);
            rb = GetComponent<Rigidbody>();
            playerControlVariables = characterState.movementControls.controlVariables;
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        if(menu == null)
        {
            menu = FindObjectOfType<Menu_Open>();
        }
    }

    private void FixedUpdate()
    { 
        if (logger == null)
        {
            logger = gameObject.GetComponent<CommandLogger>();
        }

        if(isPlayer)
        {
            rotatedMove = transform.forward * moveVector.y;
            rotatedMove += transform.right * moveVector.x;
            bool notStationary = Mathf.Abs(moveVector.x) >= .001 || Mathf.Abs(moveVector.y) >= .001;
            notStationary = notStationary || Mathf.Abs(rb.velocity.x) >= .001 || Mathf.Abs(rb.velocity.z) >= .001;

            if ((notStationary && characterState.GetCanMove()) || characterState.shouldDash || characterState.shouldJump)
            {
                HandleMovement();
            }

            transform.rotation = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0);
            anim.SetFloat("Speed", rb.velocity.magnitude);
        }

        if(holdingAbility || holdingAbility != lastHolding)
        {
            logger.AddCommandAndExecute(new UseAugment(gameObject, GameTimer.GlobalTimer.time, holdingAbility));
        }
        lastHolding = holdingAbility;
    }

    private void OnEnable()
    {
        if (inputAction != null)
        {
            inputAction.Enable();
        }
    }

    private void OnDisable()
    {
        if (inputAction != null)
        {
            inputAction.Disable();
        }
    }

    private void HandleMovement()
    {
        Vector3 sumVelocity = rotatedMove * playerControlVariables.speedPerFrame;
        if (characterState.isGrounded)
        {
            if (characterState.shouldDash)
            {
                StartCoroutine(Dash(playerControlVariables.dashDuration, (rotatedMove != Vector3.zero) ? rotatedMove : transform.forward, playerControlVariables.dashSpeed));
            }
            if (characterState.shouldJump)
            {
                Debug.Log("Jumping");
                sumVelocity.y += playerControlVariables.jumpStrength;
                jumpUpParticles = GameObject.Find("Spearman/AttackObjects/JumpUpParticles").GetComponent<ParticleSystem>();
                jumpUpParticles.Play();
            }
            else
            {
                sumVelocity.y = rb.velocity.y;
            }
        }
        else
        {
            //sumVelocity *= playerControlVariables.airSpeed;
            //if (rb.velocity.sqrMagnitude > playerControlVariables.maxAirSpeed * playerControlVariables.maxAirSpeed)
            //{
            //    sumVelocity.Normalize();
            //    sumVelocity *= playerControlVariables.maxAirSpeed;
            //}
            sumVelocity.y = rb.velocity.y;
        }

        characterState.shouldDash = false;
        characterState.shouldJump = false;
        rb.velocity = sumVelocity;
    }

    IEnumerator Dash(float duration, Vector3 direction, float speed)
    {
        float sum = 0;
        ((SpearmanState)characterState).SetState(CharacterState.CharacterStates.DASHING);
        do
        {
            RaycastHit hit;
            Physics.Raycast(GameObject.Find("warden:skel_chest").transform.position, direction, out hit, speed * Time.fixedDeltaTime, 1 << 0);
            if (hit.collider == null)
            {
                transform.position += direction * speed * Time.fixedDeltaTime;
                sum += Time.deltaTime;
            }
            else
            {
                sum = duration;
            }
            yield return new WaitForFixedUpdate();
        } while (sum < duration);
        ((SpearmanState)characterState).SetState(CharacterState.CharacterStates.IDLE);
    }

    public void InterpretKeyboardInput(char input, KeyState state)
    {
        if(menu != null && menu.touching && input == '_')
        {
            menu.OpenMenu();
            characterState.SetState(CharacterState.CharacterStates.STUNNED);
            processInput = false;
        }
        if (processInput)
        {
            if (input == '_')
            {
                if (characterState.isGrounded)
                {
                    characterState.shouldJump = true;
                }
            }
            else if (input == 'h')
            {
                characterState.shouldDash = true;
            }
            else if (input == 'Q')
            {
                skillSet.ChangeWeapon(gameObject);
            }
            else
            {
                characterState.skillSet.HandleButtonEvent(input, state);

            }
        }
        if (menu != null && menu.touching && input == 'h')
        {

            menu.CloseMenu();
            processInput = true;

        }
    }

    public void InterpretMouseInput(char input, KeyState state)
    {
        if (processInput)
        {
            if (input == '1')
            {
                holdingAbility = state == KeyState.DOWN;
            }
            else
            {
                skillSet.HandleMouseEvent(input, state);
            }
        }
    }

    public void InterpretMouseMoveEvent(InputAction.CallbackContext ctx)
    {
        if (processInput)
        {
            lookAxis = ctx.ReadValue<Vector2>();
            //logger.AddCommand(new RotateCommand(gameObject, Camera.main.transform.rotation, GameTimer.GlobalTimer.time));
        }
    }

    public float GetAxisCustom(string axisName)
    {
        if(axisName == "VerticalCam")
        {
            return lookAxis.y;
        }
        else if(axisName == "HorizontalCam")
        {
            return lookAxis.x;
        }
        return 0;
    }
}
