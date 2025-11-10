using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform Player;
    public float Speed = 3.0f;
    public float FollowDistance = 2.0f;
    public float DetectionRange = 5.0f;
    public GameObject bomb;
    public int numberOfBombs = 10;

    //public GameObject NewEnemy;
    private bool HasSpawned = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SommonAC130Strike());

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
                //if (HasSpawned == false)
                //{
                //    HasSpawned = true;
                //    GameObject NewEnemySummon = Instantiate(NewEnemy, transform.position, transform.rotation);
                //}

               

                if (distance > FollowDistance && distance > 1)  

                {
                    //Normalizes the direction vector and transforms the enemies position to the players
                    direction.Normalize();
                    transform.position += (Vector3)(direction * Speed * Time.deltaTime);
                }

                //if (distance <= FollowDistance)
                //{
                //    direction.Normalize();
                //    transform.position -= (Vector3)(direction * Speed * Time.deltaTime);
                //}
            }

        }
    }

    void AC130(GameObject nuke = null)
    {

        if (numberOfBombs > 0)
        {
            GameObject newGameObject = Instantiate(nuke, transform.position, transform.rotation);
        }
    }



    IEnumerator SommonAC130Strike()
    {

       GameObject nuke = bomb;
        while (true)
        {
            if (numberOfBombs > 0)
            {
                GameObject newGameObject = Instantiate(nuke, transform.position, transform.rotation);
            }
            yield return new WaitForSeconds(2.0f);
        }
    }



}
