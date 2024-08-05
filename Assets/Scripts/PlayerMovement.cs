using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private int normalSpeed = 4;
    private int slowSpeed = 2;
    public int currentSpeed;
    private Vector2 movement;
    private Vector2 lastNonZeroMovement;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        currentSpeed = normalSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastNonZeroMovement = Vector2.right;

        // Set the default sprite for facing right
        spriteRenderer.sprite = SelectedCharacters.Player1Character.spriteRight;
    }

    // Handle movement input
    private void OnMovement(InputValue value)
    {
        Vector2 inputMovement = value.Get<Vector2>();

        // Prevent diagonal movement
        if (Mathf.Abs(inputMovement.x) > 0 && Mathf.Abs(inputMovement.y) > 0)
        {
            if (Mathf.Abs(inputMovement.x) > Mathf.Abs(inputMovement.y))
            {
                inputMovement.y = 0;
            }
            else
            {
                inputMovement.x = 0;
            }
        }

        movement = inputMovement;

        // Update last non-zero movement
        if (movement != Vector2.zero)
        {
            lastNonZeroMovement = movement;
        }

        // Change sprite based on movement direction
        if (movement.x > 0)
        {
            spriteRenderer.sprite = SelectedCharacters.Player1Character.spriteRight;
        }
        else if (movement.x < 0)
        {
            spriteRenderer.sprite = SelectedCharacters.Player1Character.spriteLeft;
        }
        else if (movement.y > 0)
        {
            spriteRenderer.sprite = SelectedCharacters.Player1Character.spriteUp;
        }
        else if (movement.y < 0)
        {
            spriteRenderer.sprite = SelectedCharacters.Player1Character.spriteDown;
        }
    }

    private void FixedUpdate()
    {
        // Apply movement
        rb.MovePosition(rb.position + currentSpeed * Time.fixedDeltaTime * movement);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Slow down in water
        if (other.CompareTag("Water"))
        {
            currentSpeed = slowSpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Return to normal speed when exiting water
        if (other.CompareTag("Water"))
        {
            currentSpeed = normalSpeed;
        }
    }

    // Get the current facing direction
    public Vector2 GetFacingDirection()
    {
        return movement != Vector2.zero ? movement : lastNonZeroMovement;
    }
}
