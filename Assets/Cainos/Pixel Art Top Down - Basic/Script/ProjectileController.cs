using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Vector2 direction;
    private 

    void Start()
    {
        Debug.Log("I am Start");
        Debug.Log("My Start Direction " + direction);
        rigidBody.linearVelocity = direction;
        StartCoroutine(CountTo10());

    }

    IEnumerator CountTo10()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    public void SetDirection(Vector2 newDirection)
    {
        Debug.Log("I am SetDirection " + newDirection);
        direction = newDirection;
        Debug.Log("Set Direction" + direction);
    }
    
    void Awake()
    {
        Debug.Log("I am Awake");
        Debug.Log("My Awake Direction " + direction);
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("Direction" + direction);
       // if (direction.magnitude > 0.0f)
        //{
         //   Shoot();
       // }
        
    }
}