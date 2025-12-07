using Cainos.PixelArtTopDown_Basic;
using UnityEngine;

public class AttackCDPickup : MonoBehaviour
{
    public float cooldownReduction = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody2D RigidBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            // Assuming the player has a script with a method to add bombs
            TopDownCharacterController playerController = collision.gameObject.GetComponent<TopDownCharacterController>();
            if (playerController != null)
            {
                if(playerController.hasAttackCooldownUpgrade)
                {
                    Destroy(gameObject); // Destroy the pickup after being collected
                    print("Player already has the upgrade.");
                    return; // Player already has the upgrade, do nothing
                }
                playerController.attackCD -= cooldownReduction;
                playerController.hasAttackCooldownUpgrade = true;
                Destroy(gameObject); // Destroy the pickup after being collected

            }
        }
    }
}
