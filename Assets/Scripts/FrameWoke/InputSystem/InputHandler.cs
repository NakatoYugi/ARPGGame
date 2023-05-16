using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;

    public float mouseX;
    public float mouseY;

    public bool b_Input;
    public bool rb_Input;
    public bool rt_Input;
    public bool d_Pad_Up;
    public bool d_Pad_Down;
    public bool d_Pad_Left;
    public bool d_Pad_Right;
    public bool a_Input;
    public bool jump_Input;
    public bool continue_Input;
    public bool inventory_Input;

    public bool rollFlag;
    public bool inentoryFlag;
    public bool comboFlag;
    public bool sprintingFlag;
    public float rollInputTimer;

    public static InputHandler instance;

    PlayerControls inputActions;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerManager playerManager;
    UIHandler uiManager;

    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Found more than one InputHandler in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        uiManager = FindObjectOfType<UIHandler>();
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += AC => movementInput = AC.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += AC => cameraInput = AC.ReadValue<Vector2>();
        }

        inputActions.Enable();

        inputActions.PlayerActions.Jump.performed += i => continue_Input = true;
        inputActions.PlayerActions.Jump.canceled += i => continue_Input = false;
    }

    public void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        TickInput(delta);
    }

    private void LateUpdate()
    {
        rollFlag = false;
        sprintingFlag = false;
        rb_Input = false;
        rt_Input = false;
        d_Pad_Up = false;
        d_Pad_Down = false;
        d_Pad_Left = false;
        d_Pad_Right = false;
        a_Input = false;
        jump_Input = false;
        inventory_Input = false;
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleRollInput(delta);
        HandleAttackInput(delta);
        HandleQuickSlots();
        HandleInteractableButtonInput();
        HandleJumpInput();
        HandleInventoryInput();
    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void HandleRollInput(float delta)
    {
        //新输入系统
        b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;

        //长按Shift是冲刺，轻击一下是翻滚
        if (b_Input)
        {
            rollInputTimer += delta;
            sprintingFlag = true;
        }
        else
        {
            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                sprintingFlag = false;
                rollFlag = true;
            }

            rollInputTimer = 0f;
        }
    }

    private void HandleAttackInput(float delta)
    {
        inputActions.PlayerActions.RB.performed += i => rb_Input = true;
        inputActions.PlayerActions.RT.performed += i => rt_Input = true;

        if (rb_Input)
        {
            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleComboAttack(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting) return;
                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }
        }
        if (rt_Input)
        {
            playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
        }
    }

    private void HandleQuickSlots()
    {
        inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
        inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;

        if (d_Pad_Left)
        {
            playerInventory.ChangeLeftHandWeapon();
        }

        if (d_Pad_Right)
        {
            playerInventory.ChangRightHandWeapon();
        }
    }

    private void HandleInteractableButtonInput()
    {
        inputActions.PlayerActions.A.performed += i => a_Input = true;
    }

    private void HandleJumpInput()
    {
        inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
    }

    private void HandleInventoryInput()
    {
        inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;

        if (inventory_Input)
        {
            inentoryFlag = !inentoryFlag;

            if (inentoryFlag)
            {
                uiManager.OpenSelectWindow();
                uiManager.UpdateUI();
                uiManager.hudWindow.SetActive(false);
            }
            else
            {
                uiManager.hudWindow.SetActive(true);
                uiManager.CloseSelectWindow();
                uiManager.CloseAllWeaponInventoryWindow();
            }
        }
    }

    public bool GetContinuePressed()
    {
        bool result = continue_Input;
        continue_Input = false;
        return result;
    }
}