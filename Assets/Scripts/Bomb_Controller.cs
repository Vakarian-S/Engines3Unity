using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb_Controller : MonoBehaviour, IPushable
{
    public GameObject BOOM;
    [SerializeField] private SpriteRenderer bombSpriteRenderer;
    [SerializeField] private Color startColor = Color.white;
    [SerializeField] private Color tickColor = Color.red;
    [SerializeField] private float tickingSpeed = 4f;
    [SerializeField] private Rigidbody2D rigidbody2DComponent;

    private bool _isTicking = false;
    public float explodeAfterSeconds = 3f;
    public float radius = 10.0f; // explosion radius in world units
    public float damage = 5.0f; // base damage
    public float explosionForce = 10.0f; // optional physics force
    public LayerMask affectedLayers = ~0; // default: everything (use a mask to optimize)

    private void Awake()
    {
        if (bombSpriteRenderer == null)
        {
            bombSpriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        if (rigidbody2DComponent == null)
        {
            rigidbody2DComponent = GetComponent<Rigidbody2D>();
        }

        StartTicking();
        StartCoroutine(DestroyAfterSeconds(3.0f));
    }

    private void Update()
    {
        if (!_isTicking)
        {
            return;
        }


        var lerpValue = Mathf.PingPong(Time.time * tickingSpeed, 1f);
        bombSpriteRenderer.color = Color.Lerp(startColor, tickColor, lerpValue);
    }

    public void StartTicking()
    {
        _isTicking = true;
    }

    public void StopTicking()
    {
        _isTicking = false;
        bombSpriteRenderer.color = startColor;
    }

    private System.Collections.IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StopTicking();
        var newBoom = Instantiate(BOOM, transform.position, transform.rotation);
        newBoom.transform.localScale = new Vector3(radius * 3, radius * 3, newBoom.transform.localScale.z);
        Explode();

        Destroy(gameObject);
    }

    private void Explode()
    {
        // Get all colliders inside radius (filtered by layers)
        var enemies = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var obj in enemies)
        {
            var dist = Vector3.Distance(transform.position, obj.transform.position);
            if (!(dist <= radius)) continue;
            if (!obj) continue;

            // Correct usage of TryGetComponent for IDamageable
            if (obj.CompareTag("Player"))
            {
                continue;
            }

            if (obj.gameObject.TryGetComponent<IDamageable>(out var dmgComp))
            {
                dmgComp.TakeDamage((int)damage);
            }

            // Apply physics impulse if object has a Rigidbody2D
            var rb2d = obj.GetComponent<Rigidbody2D>();
            if (!rb2d) continue;
            // Calculate direction and force
            var explosionDir = (rb2d.position - (Vector2)transform.position).normalized;
            var force = explosionForce * (1.0f - (dist / radius));
            if (dist <= radius) rb2d.AddForce(explosionDir * force, ForceMode2D.Impulse);
        }
    }

    // Visualize explosion radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.4f, 0.0f, 0.3f);
        Gizmos.DrawSphere(transform.position, radius);
        Gizmos.color = new Color(1f, 0f, 0f, 0.9f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    
    public void TakeForce(Vector2 force)
    {
        // Since force is a Vector3, convert to Vector2 for 2D physics
        rigidbody2DComponent.AddForce(force.normalized * 25.0f, ForceMode2D.Impulse);
    }
}