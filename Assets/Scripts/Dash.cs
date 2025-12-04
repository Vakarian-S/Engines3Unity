using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed = 10f;
    public float dashDuration = 0.4f;
    public float dashCooldown = 1f;

    private bool isDashing = false;
    private bool canDash = true;
    private bool isInvulnerable = false;
    private Rigidbody2D rb;
    private Vector2 dashDirection;

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
        isInvulnerable = true;

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        isDashing = false;
        isInvulnerable = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // Example method to check invulnerability
    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }
}
