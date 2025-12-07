using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float dashInvulnerabilityDuration = 0.1f;
    private bool isDashing = false;
    private bool canDash = true;
    private Rigidbody2D rb;
    private Vector2 dashDirection;

    public Health healthControl;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Example: Dash when pressing Left Shift
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            if (dashDirection != Vector2.zero)
                StartCoroutine(Dash());
        }
    }

    System.Collections.IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        StartCoroutine(InvulnerabilityIFrames());

        float startTime = Time.time;
        float totalDashTime = startTime + dashDuration;
       
        while (Time.time < totalDashTime)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    System.Collections.IEnumerator InvulnerabilityIFrames()
    {
        healthControl.isInvulnerable = true;

        yield return new WaitForSeconds(dashInvulnerabilityDuration);

        healthControl.isInvulnerable = false;
    }
}
