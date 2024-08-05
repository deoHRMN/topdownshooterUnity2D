using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject shooter; // The player who shot the bullet
    public float damageAmount; // Amount of damage the bullet inflicts

    void Start()
    {
        // Destroy the bullet after a certain time to avoid it floating around forever
        Destroy(gameObject, 5f); // Adjust the time as needed
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is not the shooter and not a water area
        if (collision.gameObject != shooter && !collision.gameObject.CompareTag("Water"))
        {
            // Check if the collided object has a HealthController component
            HealthController healthController = collision.gameObject.GetComponent<HealthController>();
            if (healthController != null)
            {
                // Inflict damage to the collided object
                healthController.TakeDamage(damageAmount);
            }

            // Destroy the bullet
            Destroy(gameObject);
        }
    }

    // Optionally handle trigger collisions if you use triggers
    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the collided object is not the shooter and not a water area
        if (collider.gameObject != shooter && !collider.gameObject.CompareTag("Water"))
        {
            // Check if the collided object has a HealthController component
            HealthController healthController = collider.gameObject.GetComponent<HealthController>();
            if (healthController != null)
            {
                // Inflict damage to the collided object
                healthController.TakeDamage(damageAmount);
            }

            // Destroy the bullet
            Destroy(gameObject);
        }
    }
}
