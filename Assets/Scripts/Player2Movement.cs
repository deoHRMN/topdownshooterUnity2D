using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Movement : MonoBehaviour
{
    public int normalSpeed = 4;
    public int slowedSpeed = 2;
    public int currentSpeed;
    private Vector2 movement;
    private Vector2 lastNonZeroMovement;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public bool isInWater; // Track if the player is in water

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        currentSpeed = normalSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastNonZeroMovement = Vector2.left;

        // Set the default sprite for facing left
        spriteRenderer.sprite = SelectedCharacters.Player2Character.spriteLeft;
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
            spriteRenderer.sprite = SelectedCharacters.Player2Character.spriteRight;
        }
        else if (movement.x < 0)
        {
            spriteRenderer.sprite = SelectedCharacters.Player2Character.spriteLeft;
        }
        else if (movement.y > 0)
        {
            spriteRenderer.sprite = SelectedCharacters.Player2Character.spriteUp;
        }
        else if (movement.y < 0)
        {
            spriteRenderer.sprite = SelectedCharacters.Player2Character.spriteDown;
        }
    }

    private void FixedUpdate()
    {
        // Apply movement
        Vector2 newPosition = rb.position + movement * currentSpeed * Time.fixedDeltaTime;
        Debug.Log($"Player2 Position: {rb.position}, New Position: {newPosition}, Speed: {currentSpeed}");
        rb.MovePosition(newPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Slow down in water
        if (other.CompareTag("Water"))
        {
            currentSpeed = slowedSpeed;
            isInWater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Return to normal speed when exiting water
        if (other.CompareTag("Water"))
        {
            currentSpeed = normalSpeed;
            isInWater = false;
        }
    }

    // Get the current facing direction
    public Vector2 GetFacingDirection()
    {
        return movement != Vector2.zero ? movement : lastNonZeroMovement;
    }
}
