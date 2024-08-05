using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;
    public float powerShotCooldown = 10f; // Cooldown duration for PowerShot
    public float dashCooldown = 10f; // Cooldown duration for Dash
    public float healingCooldown = 10f; // Cooldown duration for Heal
    public float dashDuration = 5f; // Duration of Dash effect in seconds
    public int dashSpeedMultiplier = 3; // Speed multiplier for Dash

    private float lastPowerShotTime;
    private float lastDashTime;
    private float lastHealingTime;

    private PlayerShoot playerShoot;
    private Player2Shoot player2Shoot;
    private PlayerMovement playerMovement;
    private Player2Movement player2Movement;
    private HealthController healthController;
    private Rigidbody2D rb;

    private InputAction powerUpAction;

    // UI Elements
    public SpriteRenderer powerUpBar; // Reference to the SpriteRenderer representing the power-up bar

    private Vector3 originalScale;
    private Vector3 originalPosition;

    void Awake()
    {
        // Initialize components
        playerShoot = GetComponent<PlayerShoot>();
        player2Shoot = GetComponent<Player2Shoot>();
        playerMovement = GetComponent<PlayerMovement>();
        player2Movement = GetComponent<Player2Movement>();
        healthController = GetComponent<HealthController>();
        rb = GetComponent<Rigidbody2D>();

        // Get the PlayerInput component and find the PowerUp action
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found on " + gameObject.name);
            return;
        }

        powerUpAction = playerInput.actions["PowerUp"];
        if (powerUpAction == null)
        {
            Debug.LogError("PowerUp action not found in PlayerInputActions");
            return;
        }

        // Initialize cooldowns to make abilities available immediately at start
        lastPowerShotTime = Time.time - powerShotCooldown;
        lastDashTime = Time.time - dashCooldown;
        lastHealingTime = Time.time - healingCooldown;

        // Store the original scale and position of the power-up bar
        if (powerUpBar != null)
        {
            originalScale = powerUpBar.transform.localScale;
            originalPosition = powerUpBar.transform.localPosition;
        }
    }

    void OnEnable()
    {
        // Enable the PowerUp action when the object is enabled
        if (powerUpAction != null)
        {
            powerUpAction.performed += OnPowerUpPerformed;
            powerUpAction.Enable();
            Debug.Log("PowerUp action enabled for " + gameObject.name);
        }
    }

    void OnDisable()
    {
        // Disable the PowerUp action when the object is disabled
        if (powerUpAction != null)
        {
            powerUpAction.performed -= OnPowerUpPerformed;
            powerUpAction.Disable();
            Debug.Log("PowerUp action disabled for " + gameObject.name);
        }
    }

    void Update()
    {
        // Update the power-up bar UI element
        UpdatePowerUpBar();
    }

    private void OnPowerUpPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"PowerUp performed for {gameObject.name} at {Time.time}");
        switch (powerUpType)
        {
            case PowerUpType.PowerShot:
                Debug.Log($"Checking PowerShot cooldown: {Time.time - lastPowerShotTime} >= {powerShotCooldown}");
                // Check if PowerShot cooldown has elapsed
                if (Time.time - lastPowerShotTime >= powerShotCooldown)
                {
                    if (playerShoot != null)
                    {
                        Debug.Log("PowerShot activated for " + gameObject.name);
                        playerShoot.FirePoweredShot();
                    }
                    else if (player2Shoot != null)
                    {
                        Debug.Log("PowerShot activated for " + gameObject.name);
                        player2Shoot.FirePoweredShot();
                    }
                    else
                    {
                        Debug.LogError("PlayerShoot component is null for both players");
                    }
                    lastPowerShotTime = Time.time;
                }
                else
                {
                    Debug.Log("PowerShot is still on cooldown for " + gameObject.name);
                }
                break;

            case PowerUpType.Dash:
                Debug.Log($"Checking Dash cooldown: {Time.time - lastDashTime} >= {dashCooldown}");
                // Check if Dash cooldown has elapsed
                if (Time.time - lastDashTime >= dashCooldown)
                {
                    StartCoroutine(Dash());
                    lastDashTime = Time.time;
                }
                else
                {
                    Debug.Log("Dash is still on cooldown for " + gameObject.name);
                }
                break;

            case PowerUpType.Heal:
                Debug.Log($"Checking Heal cooldown: {Time.time - lastHealingTime} >= {healingCooldown}");
                // Check if Heal cooldown has elapsed
                if (Time.time - lastHealingTime >= healingCooldown)
                {
                    Heal();
                    lastHealingTime = Time.time;
                }
                else
                {
                    Debug.Log("Heal is still on cooldown for " + gameObject.name);
                }
                break;
        }
    }

    private IEnumerator Dash()
    {
        Debug.Log("Dash effect started for " + gameObject.name);
        // Apply Dash effect by increasing speed
        if (playerMovement != null)
        {
            playerMovement.currentSpeed *= dashSpeedMultiplier;
            yield return new WaitForSeconds(dashDuration);
            playerMovement.currentSpeed /= dashSpeedMultiplier;
            Debug.Log("Dash effect ended for " + gameObject.name);
        }
        else if (player2Movement != null)
        {
            player2Movement.currentSpeed *= dashSpeedMultiplier;
            yield return new WaitForSeconds(dashDuration);
            player2Movement.currentSpeed /= dashSpeedMultiplier;
            Debug.Log("Dash effect ended for " + gameObject.name);
        }
        else
        {
            Debug.LogError("PlayerMovement component is null for both players");
        }
    }

    private void Heal()
    {
        Debug.Log("Heal effect applied for " + gameObject.name);
        // Heal the player by a fixed amount
        if (healthController != null)
        {
            healthController.Heal(2); // Heal 2 HP
        }
        else
        {
            Debug.LogError("HealthController component is null");
        }
    }

    // Update the power-up bar UI to reflect cooldown status
    private void UpdatePowerUpBar()
    {
        if (powerUpBar != null)
        {
            float cooldownTime = 0;
            switch (powerUpType)
            {
                case PowerUpType.PowerShot:
                    cooldownTime = powerShotCooldown;
                    break;
                case PowerUpType.Dash:
                    cooldownTime = dashCooldown;
                    break;
                case PowerUpType.Heal:
                    cooldownTime = healingCooldown;
                    break;
            }

            // Calculate fill amount based on cooldown progress
            float elapsed = Time.time - GetLastUsedTime();
            float fillAmount = elapsed < cooldownTime ? elapsed / cooldownTime : 1;

            // Update the power-up bar scale and position
            powerUpBar.transform.localScale = new Vector3(fillAmount * originalScale.x, originalScale.y, originalScale.z);
            powerUpBar.transform.localPosition = new Vector3(originalPosition.x - (originalScale.x - powerUpBar.transform.localScale.x) / 2, originalPosition.y, originalPosition.z);
        }
    }

    // Get the last used time of the current power-up type
    private float GetLastUsedTime()
    {
        switch (powerUpType)
        {
            case PowerUpType.PowerShot:
                return lastPowerShotTime;
            case PowerUpType.Dash:
                return lastDashTime;
            case PowerUpType.Heal:
                return lastHealingTime;
            default:
                return 0;
        }
    }
}
