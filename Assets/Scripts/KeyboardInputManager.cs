using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script
{
    public class KeyboardInputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;

        public static KeyboardInputManager Instance { get; private set; }
        public Action<Vector2> MoveAction;
        public Action<Vector2> LookAction;
        public Action JumpAction;
        public Action CrouchAction;
        public Action<bool> SprintAction;
        public Action InteractAction;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            playerInput ??= GetComponent<PlayerInput>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                MoveAction?.Invoke(context.ReadValue<Vector2>());
            }
            else if (context.canceled)
            {
                MoveAction?.Invoke(Vector2.zero);
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookAction?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                JumpAction?.Invoke();
            }
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                CrouchAction?.Invoke();
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SprintAction?.Invoke(true);
            }
            else if (context.canceled)
            {
                SprintAction?.Invoke(false);
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                InteractAction?.Invoke();
            }
        }
    }
}