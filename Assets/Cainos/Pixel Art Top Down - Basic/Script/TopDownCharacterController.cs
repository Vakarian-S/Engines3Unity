using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;


namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        public float speed;

        private Animator animator;
        InputAction moveAction;
        private InputAction attackAction;

        private Rigidbody2D rigidBody;
        public GameObject projectile;

        private void Start()
        {
            animator = GetComponent<Animator>();
            moveAction = InputSystem.actions.FindAction("Move");
            moveAction.Enable();
            attackAction = InputSystem.actions.FindAction("Attack");
            attackAction.Enable();
            rigidBody = GetComponent<Rigidbody2D>();
        }


        private void Update()
        {
            Vector2 dir = Vector2.zero;
            Vector2 moveValue = moveAction.ReadValue<Vector2>();
            float attackValue = attackAction.ReadValue<float>();
            
            dir.x = moveValue.x;
            dir.y = moveValue.y;
            dir.Normalize();
            animator.SetBool("IsMoving", dir.magnitude > 0);
            
            
            if (Mathf.Approximately(attackValue, 1.0f) && attackAction.WasPressedThisFrame())
            {
                GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity );
                projectileInstance.GetComponent<ProjectileController>().SetDirection(new Vector2(5.0f,0.0f));
                //projectile.GetComponent<ProjectileController>().SetDirection(new Vector2(0.0f, 5.0f));
                //projectile.GetComponent<ProjectileController>().SetDirection(new Vector2(0.0f, 5.0f));
            }
            
            rigidBody.linearVelocity = speed * dir;
        }
    }
}