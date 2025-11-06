using Unity.VisualScripting;
using UnityEngine;

public class Bomb_Controller : MonoBehaviour
{
    public GameObject BOOM;

    public float explodeAfterSeconds = 3f;
    public float radius = 10.0f;            // explosion radius in world units
    public float damage = 5.0f;           // base damage
    public float explosionForce = 10.0f;  // optional physics force
    public LayerMask affectedLayers = ~0; // default: everything (use a mask to optimize)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DestroyAfterSeconds(3.0f));

        
    }

    void Update()
    {

    }

    private System.Collections.IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
       GameObject newBOOM = Instantiate(BOOM, transform.position, transform.rotation);
        
        Explode();

        Destroy(gameObject);
    }
    private void Explode()
    {
        // Get all colliders inside radius (filtered by layers)
        GameObject[] enemies = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var obj in enemies)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist <= radius)
            {
                    Debug.Log($"Checking object: {obj.name}");
                    if (obj == null) continue;

                    // Correct usage of TryGetComponent for IDamageable
                    if (obj.gameObject.TryGetComponent<IDamageable>(out var dmgComp))
                    {
                        Debug.Log($"Applying {damage} damage to {obj.name}");
                        dmgComp.TakeDamage((int)damage);
                    }
                    // Apply physics impulse if object has a Rigidbody2D
                    var rb2d = obj.GetComponent<Rigidbody2D>();
                    if (rb2d != null)
                    {
                        // Calculate direction and force
                        Vector2 explosionDir = (rb2d.position - (Vector2)transform.position).normalized;
                        float force = explosionForce * (1.0f - (dist / radius));
                    if(dist <= radius) rb2d.AddForce(explosionDir * force, ForceMode2D.Impulse);
                    }
            }   
                
            
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
}
