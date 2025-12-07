using UnityEngine;
using System.Collections;

public class ArcScale : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(1f, 1f, 1f); // maximum scale
    public float growTime = 0.5f; // duration to reach full scale

    private Vector3 initialScale = Vector3.zero;

    void OnEnable()
    {
        // Reset scale when the object is spawned
        transform.localScale = initialScale;

        // Start scaling coroutine
        StartCoroutine(GrowArc());
    }

    private IEnumerator GrowArc()
    {
        float elapsed = 0f;

        while (elapsed < growTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / growTime;
            // Optional: ease in/out
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }

        transform.localScale = targetScale; // ensure it reaches exact max scale
    }
}
