using UnityEngine;

public class CameraPan : MonoBehaviour
{
    public float radius = 50f;          
    public float speed = 1f;            
    public bool clockwise = true;       // Reverse the motion direction if needed

    public bool pulseEffect = false;
    public float scaleAmount = 0.05f;   
    public float scaleSpeed = 1f;       

    private RectTransform rectTransform;
    private Vector2 startPosition;
    private float angle = 0f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;

        // Auto zoom in to prevent edges from showing
        rectTransform.localScale = new Vector3(1.2f, 1.2f, 1f);
    }

    void Update()
    {
        angle += (clockwise ? 1 : -1) * speed * Time.deltaTime;

        float xOffset = Mathf.Cos(angle) * radius;
        float yOffset = Mathf.Sin(angle) * radius;

        rectTransform.anchoredPosition = startPosition + new Vector2(xOffset, yOffset);

        if (pulseEffect)
        {
            float scale = 1.4f + Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;
            rectTransform.localScale = new Vector3(scale, scale, 1f);
        }
    }
}
