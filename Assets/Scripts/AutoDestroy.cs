using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 0.5f); // adjust duration
    }

    void Update()
    {
    }
}