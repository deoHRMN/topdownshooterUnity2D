using UnityEngine;
using TMPro;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float _currentHealth;

    [SerializeField]
    private float _maximumHealth = 10f;

    [SerializeField]
    private TextMeshPro _healthText;

    private GameController gameController;

    void Start()
    {
        _currentHealth = _maximumHealth;
        UpdateHealthText();
        gameController = Object.FindFirstObjectByType<GameController>();

        if (gameController == null)
        {
            Debug.LogError("GameController not found in the scene.");
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (_currentHealth == 0)
        {
            return;
        }

        _currentHealth -= damageAmount;

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        UpdateHealthText();

        if (_currentHealth == 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        if (_currentHealth == _maximumHealth)
        {
            return; // Already at maximum health
        }

        _currentHealth += healAmount;

        if (_currentHealth > _maximumHealth)
        {
            _currentHealth = _maximumHealth; // Don't exceed maximum health
        }

        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        if (_healthText != null)
        {
            _healthText.text = _currentHealth.ToString();
        }
    }

    private void Die()
    {
        if (gameController != null)
        {
            gameController.PlayerDied(gameObject.tag); // Use tag instead of name
        }
        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} has died.");
    }
}
