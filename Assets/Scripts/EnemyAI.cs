using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform Player;
    public float Speed = 3.0f;
    public float FollowDistance = 2.0f;
    public float DetectionRange = 5.0f;
    public GameObject NewEnemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //Checks if the player is there
        if (Player != null)
        {
            

            //Calculates the direction to the player    
            Vector2 direction = Player.position - transform.position;
            //Calculates the distance to the player by getting the magnitude of the direction vector
            float distance = direction.magnitude;
            if (distance <= DetectionRange)
            {

                GameObject NewEnemySummon = Instantiate(NewEnemy, transform.position, transform.rotation);
                if (distance > FollowDistance)  
                {
                    //Normalizes the direction vector and transforms the enemies position to the players
                    direction.Normalize();
                    transform.position += (Vector3)(direction * Speed * Time.deltaTime);
                }

                if (distance <= FollowDistance)
                {
                    direction.Normalize();
                    transform.position -= (Vector3)(direction * Speed * Time.deltaTime);
                }
            }

        }
    }
}
