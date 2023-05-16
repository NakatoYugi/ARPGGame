using UnityEngine;

public class PlayerManager :MonoBehaviour, IDataPersistence
{
    InputHandler inputHandler;
    Animator anim;
    CameraHandler cameraHandler;
    PlayerLocomotion playerLocomotion;
    InteractableUI interactablePopUpUI;
    public GameObject interactableUIGameObject;
    public GameObject interactableItemGameObject;

    [Header("Player Flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;

    private float interactableItemActiveTimer;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        anim = GetComponentInChildren<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        cameraHandler = FindObjectOfType<CameraHandler>();
        interactablePopUpUI = FindObjectOfType<InteractableUI>();

        interactableItemGameObject.SetActive(false);
    }

    void Start()
    {
        AnimatorClipInfo[] clips = anim.GetCurrentAnimatorClipInfo(1);
        foreach (AnimatorClipInfo clip in clips)
        {
            AnimationClip _clip = clip.clip;
            Debug.Log(_clip.name);
        }
    }

    private void Update()
    {
        anim.SetBool("isInAir", isInAir);
        isInteracting = anim.GetBool("isInteracting");
        canDoCombo = anim.GetBool("canDoCombo");

        float delta = Time.deltaTime;
        //inputHandler.TickInput(delta);
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        playerLocomotion.HandleJumping();

        CheckForInteractableObject();

        //设置拾取物体后 屏幕提示信息到时的消失
        if (interactableItemGameObject.activeSelf)
        {
            interactableItemActiveTimer += Time.deltaTime;
            if (interactableItemActiveTimer >= 1f)
            {
                interactableItemGameObject.SetActive(false);
                interactableItemActiveTimer = 0f;
            }
        }
    }
    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta,inputHandler.mouseX, inputHandler.mouseY);
        }
    }

    private void LateUpdate()
    {
        if (isInAir)
        {
            playerLocomotion.inAirTimer += Time.deltaTime;
        }
    }

    public void CheckForInteractableObject()
    {
        RaycastHit hit;

        //沿射线投射球体并返回有关命中对象的详细信息
        if (!Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
        {
            if (interactableUIGameObject != null)
                interactableUIGameObject.SetActive(false);
            return;
        }
            
        if (hit.collider.tag == "Interactable")
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable == null) return;

            string interactableText = interactable.interactableText;
            interactablePopUpUI.InteractableText.text = interactableText;
            if (interactableUIGameObject != null) interactableUIGameObject.SetActive(true);

            if (inputHandler.a_Input)
            {
                interactable.Interact(this);
            }
        }
    }

    public void SaveData(GameData data)
    {
        data.PlayerPosition = transform.position;
    }

    public void LoadData(GameData data)
    {
        transform.position = data.PlayerPosition;
    }
}