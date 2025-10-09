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

        private Animator _animator;
        private InputAction _moveAction;
        private InputAction _attackAction;
        private Rigidbody2D _rigidBody;
        public GameObject projectile;
        public GameObject bomb;
        public int numberOfBombs = 1;


        [SerializeField] private float attackCooldownSeconds = 0.35f;
        private bool _canShoot = true;
        
        

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _moveAction = InputSystem.actions.FindAction("Move");
            _moveAction.Enable();
            _attackAction = InputSystem.actions.FindAction("DirectionalAttack");
            _attackAction.Enable();
            _rigidBody = GetComponent<Rigidbody2D>();
        }
        
        IEnumerator AttackCooldown()
        {
            yield return new WaitForSeconds(0.5f);
            _canShoot = true;
        }

        private void SpawnObject(GameObject Spawn_Object = null){
          GameObject newGameObject = Instantiate(Spawn_Object, transform.position, transform.rotation);
        }
        private void Update()
        {
            Vector2 dir = Vector2.zero;
            Vector2 moveValue = _moveAction.ReadValue<Vector2>();
            Vector2 attackValue = _attackAction.ReadValue<Vector2>();
            
            dir.x = moveValue.x;
            dir.y = moveValue.y;
            dir.Normalize();
            _animator.SetBool("IsMoving", dir.magnitude > 0);
            
            if (
                Mathf.Approximately(attackValue.magnitude, 1.0f) &&
                _attackAction.WasPressedThisFrame() &&
                _canShoot
                )
            {
                _canShoot = false;
                
                Vector3 spawnOffset = Vector2.up * 0.5f + attackValue * 1.2f;
                GameObject projectileInstance = Instantiate(projectile, transform.position + spawnOffset, Quaternion.identity );
                projectileInstance.GetComponent<ProjectileController>().SetDirection(attackValue);
                
                StartCoroutine(AttackCooldown());
            }
            
            _rigidBody.linearVelocity = speed * dir;
        
            if(Input.GetKeyDown(KeyCode.B) && numberOfBombs > 0)
            {
                SpawnObject(bomb);
                numberOfBombs -= 1;
            }

        }
    }
}