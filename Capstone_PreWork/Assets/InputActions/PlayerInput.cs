// GENERATED AUTOMATICALLY FROM 'Assets/InputActions/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerInput : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""PlayerControls"",
            ""id"": ""80e55826-0627-4ea1-9e17-17a8f1fbebcf"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ef64da13-fe60-4eba-b74c-9fdbdb577459"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9072a152-45c5-4bb7-86fa-ddfc404f8893"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""eb2217dc-ec65-45ee-ae9c-12c8dda623c4"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""7b54e914-a3c2-4ebe-9f51-e1758500ae10"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PrimaryAttack"",
                    ""type"": ""Button"",
                    ""id"": ""2ddb8099-2b09-4615-a90b-ea15d06bd44e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondaryAttack"",
                    ""type"": ""Button"",
                    ""id"": ""11e0aff9-338d-445b-bba4-12ac35907cfc"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwapLoadout"",
                    ""type"": ""Button"",
                    ""id"": ""61d4e972-c504-4a26-99a0-f8d9a1056506"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectButton"",
                    ""type"": ""Button"",
                    ""id"": ""37ac92a9-2b76-4717-ba56-88ce0e090cae"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StartButton"",
                    ""type"": ""Button"",
                    ""id"": ""e39680dc-a2bb-48b0-b4b5-69805d3b69a1"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""33281850-9d88-44a3-a032-76fdd6f2c2f6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6132cc30-0730-4867-aefc-b7333060df36"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0617ad53-c8a8-4d49-bd91-8ba25972d19c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5d05c63b-46b9-4eaf-a580-51d79099f30a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""97edcea6-d993-40cd-a4e4-243fba851a8a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2310e07f-02a8-4023-8045-6aab94827142"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard;Controller"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""77acaa4b-b6c2-47b8-9a7e-9a161824eaa3"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9f4dcb3-1c02-4e7e-8f3a-bcfe9aac0568"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5101f5d3-5817-4928-856e-723a3ce0921c"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""86c21ddb-fc07-4e7d-a2bc-2c480d3833fd"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bd8b1d66-f41b-438f-a942-fcb4c3080e57"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1b851f0f-f9b5-483c-b0ab-6a64676ab7bc"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a8c2c95-7813-4453-af19-f701d04b9df9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PrimaryAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f4273886-1a51-4514-8748-c06fee83e2ee"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""PrimaryAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a7b62f61-56b6-40b4-ab8f-7fb8b558d3a9"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SecondaryAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""03d8d422-49ad-43b0-8b52-429b6d9d3eb9"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""SecondaryAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""88bf46d2-1583-47d5-af70-692960ab32e6"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SwapLoadout"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af9acbd8-44d9-4fa3-9a16-fa97cac9bde5"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""SwapLoadout"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5cd2d498-431a-4781-8122-f36b608e8b23"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d9aeede9-ad82-4e8e-a55c-6e3788dc6450"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StartButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerControls
        m_PlayerControls = asset.FindActionMap("PlayerControls", throwIfNotFound: true);
        m_PlayerControls_Movement = m_PlayerControls.FindAction("Movement", throwIfNotFound: true);
        m_PlayerControls_Jump = m_PlayerControls.FindAction("Jump", throwIfNotFound: true);
        m_PlayerControls_Look = m_PlayerControls.FindAction("Look", throwIfNotFound: true);
        m_PlayerControls_Dash = m_PlayerControls.FindAction("Dash", throwIfNotFound: true);
        m_PlayerControls_PrimaryAttack = m_PlayerControls.FindAction("PrimaryAttack", throwIfNotFound: true);
        m_PlayerControls_SecondaryAttack = m_PlayerControls.FindAction("SecondaryAttack", throwIfNotFound: true);
        m_PlayerControls_SwapLoadout = m_PlayerControls.FindAction("SwapLoadout", throwIfNotFound: true);
        m_PlayerControls_SelectButton = m_PlayerControls.FindAction("SelectButton", throwIfNotFound: true);
        m_PlayerControls_StartButton = m_PlayerControls.FindAction("StartButton", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // PlayerControls
    private readonly InputActionMap m_PlayerControls;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerControls_Movement;
    private readonly InputAction m_PlayerControls_Jump;
    private readonly InputAction m_PlayerControls_Look;
    private readonly InputAction m_PlayerControls_Dash;
    private readonly InputAction m_PlayerControls_PrimaryAttack;
    private readonly InputAction m_PlayerControls_SecondaryAttack;
    private readonly InputAction m_PlayerControls_SwapLoadout;
    private readonly InputAction m_PlayerControls_SelectButton;
    private readonly InputAction m_PlayerControls_StartButton;
    public struct PlayerControlsActions
    {
        private PlayerInput m_Wrapper;
        public PlayerControlsActions(PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerControls_Movement;
        public InputAction @Jump => m_Wrapper.m_PlayerControls_Jump;
        public InputAction @Look => m_Wrapper.m_PlayerControls_Look;
        public InputAction @Dash => m_Wrapper.m_PlayerControls_Dash;
        public InputAction @PrimaryAttack => m_Wrapper.m_PlayerControls_PrimaryAttack;
        public InputAction @SecondaryAttack => m_Wrapper.m_PlayerControls_SecondaryAttack;
        public InputAction @SwapLoadout => m_Wrapper.m_PlayerControls_SwapLoadout;
        public InputAction @SelectButton => m_Wrapper.m_PlayerControls_SelectButton;
        public InputAction @StartButton => m_Wrapper.m_PlayerControls_StartButton;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
            {
                Movement.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMovement;
                Movement.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMovement;
                Movement.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMovement;
                Jump.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                Jump.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                Jump.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                Look.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                Look.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                Look.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                Dash.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnDash;
                Dash.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnDash;
                Dash.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnDash;
                PrimaryAttack.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPrimaryAttack;
                PrimaryAttack.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPrimaryAttack;
                PrimaryAttack.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPrimaryAttack;
                SecondaryAttack.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSecondaryAttack;
                SecondaryAttack.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSecondaryAttack;
                SecondaryAttack.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSecondaryAttack;
                SwapLoadout.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSwapLoadout;
                SwapLoadout.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSwapLoadout;
                SwapLoadout.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSwapLoadout;
                SelectButton.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSelectButton;
                SelectButton.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSelectButton;
                SelectButton.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSelectButton;
                StartButton.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnStartButton;
                StartButton.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnStartButton;
                StartButton.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnStartButton;
            }
            m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                Movement.started += instance.OnMovement;
                Movement.performed += instance.OnMovement;
                Movement.canceled += instance.OnMovement;
                Jump.started += instance.OnJump;
                Jump.performed += instance.OnJump;
                Jump.canceled += instance.OnJump;
                Look.started += instance.OnLook;
                Look.performed += instance.OnLook;
                Look.canceled += instance.OnLook;
                Dash.started += instance.OnDash;
                Dash.performed += instance.OnDash;
                Dash.canceled += instance.OnDash;
                PrimaryAttack.started += instance.OnPrimaryAttack;
                PrimaryAttack.performed += instance.OnPrimaryAttack;
                PrimaryAttack.canceled += instance.OnPrimaryAttack;
                SecondaryAttack.started += instance.OnSecondaryAttack;
                SecondaryAttack.performed += instance.OnSecondaryAttack;
                SecondaryAttack.canceled += instance.OnSecondaryAttack;
                SwapLoadout.started += instance.OnSwapLoadout;
                SwapLoadout.performed += instance.OnSwapLoadout;
                SwapLoadout.canceled += instance.OnSwapLoadout;
                SelectButton.started += instance.OnSelectButton;
                SelectButton.performed += instance.OnSelectButton;
                SelectButton.canceled += instance.OnSelectButton;
                StartButton.started += instance.OnStartButton;
                StartButton.performed += instance.OnStartButton;
                StartButton.canceled += instance.OnStartButton;
            }
        }
    }
    public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IPlayerControlsActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnPrimaryAttack(InputAction.CallbackContext context);
        void OnSecondaryAttack(InputAction.CallbackContext context);
        void OnSwapLoadout(InputAction.CallbackContext context);
        void OnSelectButton(InputAction.CallbackContext context);
        void OnStartButton(InputAction.CallbackContext context);
    }
}
