using System.Collections;
using UnityEngine;
using Script;

namespace Polyart
{
    public class FirstPersonController_Dreamscape : MonoBehaviour
    {
        public bool CanMove { get; private set; } = true;
        private bool isSprinting;
        private bool shouldJump;
        private bool shouldCrouch;
        private bool shouldAttack;

        [Header("Character Options")]
        [SerializeField] private bool canSprint = true;
        [SerializeField] private bool canJump = true;
        [SerializeField] private bool canCrouch = true;
        [SerializeField] private bool canHeadBob = true;
        [SerializeField] private bool canInteract = true;
        [SerializeField] private bool useFootsteps = true;
        [SerializeField] private bool canAttack = true;

        [Header("Interaction")]
        [SerializeField] private Vector3 interactionRayPoint = default;
        [SerializeField] private float interactionDistance = default;
        [SerializeField] private LayerMask interactionLayer = default;
        private Interactable_Dreamscape currentInteractable;

        [Header("Movement Parameters")]
        [SerializeField] private float walkSpeed = 3.0f;
        [SerializeField] private float sprintSpeed = 6.0f;
        [SerializeField] private float crouchSpeed = 1.5f;

        [Header("Camera Parameters")]
        [SerializeField, Range(0.1f, 10)] private float lookSpeedX = 0.5f;
        [SerializeField, Range(0.1f, 10)] private float lookSpeedY = 0.5f;
        [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;
        [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;

        [Header("Jumping Parameters")]
        [SerializeField] private float jumpForce = 8.0f;
        [SerializeField] private float gravity = 30.0f;

        [Header("Crouching Parameters")]
        [SerializeField] private float crouchHeight = 0.5f;
        [SerializeField] private float standingHeight = 2f;
        [SerializeField] private float timeToCrouch = 0.25f;
        [SerializeField] private Vector3 crouchCenter = new Vector3(0, 0.5f, 0);
        [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
        private bool isCrouching;
        private bool duringCrouchAnim;

        [Header("Headbob Parameters")]
        [SerializeField] private float walkBobSpeed = 14.0f;
        [SerializeField] private float walkBobAmount = 0.05f;
        [SerializeField] private float sprintBobSpeed = 18.0f;
        [SerializeField] private float sprintBobAmount = 0.11f;

        [Header("Footstep Parameters")]
        [SerializeField] private float baseStepSpeed = 0.5f;
        [SerializeField] private float crouchStepMultiplier = 1.5f;
        [SerializeField] private float sprintStepMultiplier = 0.6f;
        [SerializeField] private AudioSource footstepAudioSource = default;
        [SerializeField] private AudioClip[] woodClips = default;
        [SerializeField] private AudioClip[] stoneClips = default;
        [SerializeField] private AudioClip[] waterClips = default;
        [SerializeField] private AudioClip[] grassClips = default;

        private float footstepTimer = 0;
        private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : isSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;

        private float defaultYPos = 0;
        private float timer;

        private Camera playerCamera;
        private CharacterController characterController;
        private Animator animator;

        private Vector3 moveDirection;
        private Vector2 currentInput;

        private float rotationX = 0;

        void Awake()
        {
            playerCamera = GetComponentInChildren<Camera>();
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            defaultYPos = playerCamera.transform.localPosition.y;
        }

        void OnEnable()
        {
            KeyboardInputManager.Instance.MoveAction += OnMove;
            KeyboardInputManager.Instance.LookAction += OnLook;
            KeyboardInputManager.Instance.JumpAction += OnJump;
            KeyboardInputManager.Instance.CrouchAction += OnCrouch;
            KeyboardInputManager.Instance.SprintAction += OnSprint;
            KeyboardInputManager.Instance.InteractAction += OnInteract;
            KeyboardInputManager.Instance.AttackAction += OnAttack;
        }

        void OnDisable()
        {
            KeyboardInputManager.Instance.MoveAction -= OnMove;
            KeyboardInputManager.Instance.LookAction -= OnLook;
            KeyboardInputManager.Instance.JumpAction -= OnJump;
            KeyboardInputManager.Instance.CrouchAction -= OnCrouch;
            KeyboardInputManager.Instance.SprintAction -= OnSprint;
            KeyboardInputManager.Instance.InteractAction -= OnInteract;
            KeyboardInputManager.Instance.AttackAction -= OnAttack;
        }

        void Update()
        {
            if (CanMove)
            {
                HandleMovementInput();
                HandleMouseLook();

                if (canJump)
                    HandleJump();

                if (canCrouch)
                    HandleCrouch();

                if (canHeadBob)
                {
                    HandleHeadBob();
                }

                if (canInteract)
                {
                    HandleInteractionCheck();
                }

                if (useFootsteps)
                {
                    HandleFootsteps();
                }

                if (canAttack)
                {
                    HandleAttack();
                }

                ApplyFinalMovements();
            }
        }

        private void OnMove(Vector2 direction)
        {
            currentInput = direction;
        }

        private void OnLook(Vector2 lookInput)
        {
            rotationX -= lookInput.y * lookSpeedY;
            rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, lookInput.x * lookSpeedX, 0);
        }

        private void OnJump()
        {
            shouldJump = true;
        }

        private void OnSprint(bool sprinting)
        {
            isSprinting = sprinting;
        }

        private void OnCrouch()
        {
            shouldCrouch = true;
        }

        private void OnInteract()
        {
            if (currentInteractable != null)
            {
                currentInteractable.OnInteract();
            }
        }

        private void OnAttack()
        {
            shouldAttack = true;
        }

        private void HandleMovementInput()
        {
            float speed = isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed;
            Vector3 forwardMovement = transform.forward * currentInput.y * speed;
            Vector3 rightMovement = transform.right * currentInput.x * speed;

            moveDirection = (forwardMovement + rightMovement).normalized * speed;
        }

        private void HandleMouseLook()
        {
            // Mouse look is handled in the OnLook method
        }

        private void HandleJump()
        {
            if (shouldJump && characterController.isGrounded)
            {
                moveDirection.y = jumpForce;
                shouldJump = false;
            }
        }

        private void HandleCrouch()
        {
            if (shouldCrouch && !duringCrouchAnim && characterController.isGrounded)
            {
                StartCoroutine(CrouchStand());
                shouldCrouch = false;
            }
        }

        private void HandleHeadBob()
        {
            if (!characterController.isGrounded) return;
            if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
            {
                timer += Time.deltaTime * (isSprinting ? sprintBobSpeed : walkBobSpeed);
                playerCamera.transform.localPosition = new Vector3(
                    playerCamera.transform.localPosition.x,
                    defaultYPos + Mathf.Sin(timer) * (isSprinting ? sprintBobAmount : walkBobAmount),
                    playerCamera.transform.localPosition.z);
            }
        }

        private void HandleAttack()
        {
            if (shouldAttack)
            {
                shouldAttack = false;
                animator.SetTrigger("MeleeAttack_TwoHanded");
            }
        }

        private void ApplyFinalMovements()
        {
            if (!characterController.isGrounded)
                moveDirection.y -= gravity * Time.deltaTime;

            characterController.Move(moveDirection * Time.deltaTime);
        }

        private IEnumerator CrouchStand()
        {
            if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
                yield break;

            duringCrouchAnim = true;

            float timeElapsed = 0;
            float targetHeight = isCrouching ? standingHeight : crouchHeight;
            float currentHeight = characterController.height;
            Vector3 targetCenter = isCrouching ? standingCenter : crouchCenter;
            Vector3 currentCenter = characterController.center;

            while (timeElapsed < timeToCrouch)
            {
                characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
                characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            characterController.height = targetHeight;
            characterController.center = targetCenter;

            isCrouching = !isCrouching;
            duringCrouchAnim = false;
        }

        private void HandleInteractionCheck()
        {
            if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
            {
                if (hit.collider.gameObject.layer == 7 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.gameObject.GetInstanceID()))
                {
                    hit.collider.TryGetComponent(out currentInteractable);
                    if (currentInteractable)
                        currentInteractable.OnFocus();
                }
            }
            else if (currentInteractable)
            {
                currentInteractable.OnLoseFocus();
                currentInteractable = null;
            }
        }

        private void HandleFootsteps()
        {
            if (!characterController.isGrounded) return;
            if (currentInput == Vector2.zero) return;

            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0)
            {
                if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 2))
                {
                    switch (hit.collider.tag)
                    {
                        case "Footsteps/Grass":
                            footstepAudioSource.PlayOneShot(grassClips[Random.Range(0, grassClips.Length - 1)]);
                            break;
                        case "Footsteps/Stone":
                            footstepAudioSource.PlayOneShot(stoneClips[Random.Range(0, stoneClips.Length - 1)]);
                            break;
                        case "Footsteps/Water":
                            footstepAudioSource.PlayOneShot(waterClips[Random.Range(0, waterClips.Length - 1)]);
                            break;
                        case "Footsteps/Wood":
                            footstepAudioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)]);
                            break;
                        default:
                            footstepAudioSource.PlayOneShot(grassClips[Random.Range(0, grassClips.Length - 1)]);
                            break;
                    }
                }

                footstepTimer = GetCurrentOffset;
            }
        }
    }
}
