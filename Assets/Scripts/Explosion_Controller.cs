using UnityEngine;

public class Explosion_Controller : MonoBehaviour
{
    [SerializeField] private GameObject spawnSoundObject;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DestroyAfterSeconds(0.5f));
        Instantiate(spawnSoundObject, transform.position, Quaternion.identity);
    }


    void Update()
    {

    }

    private System.Collections.IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Destroy(gameObject);
    }
}
