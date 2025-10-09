using System;
using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private Vector2 _direction;
    private const float Velocity = 5.0f;
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private bool destroyOnAnyHit = true;
    [SerializeField] private GameObject hitEffectPrefab; // 👈 assign this in the inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);

        // Try interface first
        if (collision.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damageAmount);
            if (destroyOnAnyHit)
            {
                Destroy(gameObject);
                SpawnHitEffect();
            }

            return;
        }

        if (!destroyOnAnyHit) return;
        Destroy(gameObject);
        SpawnHitEffect();

    }

    private void Start()
    {
        _rigidBody.linearVelocity = _direction * Velocity;
        transform.right = _direction;
        StartCoroutine(CountTo10());
    }

    private IEnumerator CountTo10()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    public void SetDirection(Vector2 newDirection)
    {
        _direction = newDirection;
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    
    
    private void SpawnHitEffect()
    {
        if (hitEffectPrefab == null) return;
        Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
    }
    
}