using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Polyperfect.Common;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Polyperfect.PrehistoricAnimals
{
    public class MovementController : MonoBehaviour
    {
        public float walkSpeed = 1f;
        public float runSpeed = 5f;
        private Animator animator;
        private CharacterController characterController;
        private PrehistoricAnimals_WanderScript wanderScript;

        void Start()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
            wanderScript = GetComponent<PrehistoricAnimals_WanderScript>();
        }

        void Update()
        {
            float moveSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            float moveVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            float moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            Vector3 moveDirection = new Vector3(moveHorizontal, 0, moveVertical);
            characterController.Move(moveDirection);

            if (moveDirection.magnitude > 0)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", Input.GetKey(KeyCode.LeftShift));
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }
        }
    }
}