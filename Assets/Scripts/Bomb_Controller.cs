using UnityEngine;

public class Bomb_Controller : MonoBehaviour
{
    public GameObject BOOM;

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
        Destroy(gameObject);
    }
}
