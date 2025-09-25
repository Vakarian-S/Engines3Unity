using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private Vector2 _direction;
    private const float Velocity = 5.0f;

    void Start()
    {
        _rigidBody.linearVelocity =  _direction * Velocity;
        transform.right = _direction;
        StartCoroutine(CountTo10());
    }

    IEnumerator CountTo10()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    public void SetDirection(Vector2 newDirection)
    {
        _direction = newDirection;
    }
    
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }
}