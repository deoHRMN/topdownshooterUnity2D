using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Shoot : MonoBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private float _bulletSpeed;

    [SerializeField]
    private float _timeBetweenShots;

    [SerializeField]
    private GameObject _shootingPoint; // Reference to the shooting point

    public float normalBulletDamage = 1f; // Normal bullet damage
    public float poweredBulletDamage = 2f; // Powered bullet damage
    private float _currentBulletDamage; // Current bullet damage

    private bool _fireContinuously;
    private float _lastFireTime;

    // Reference to the Player2Movement script to get the facing direction
    private Player2Movement _player2Movement;
    private PlayerPowerUp _playerPowerUp; // Reference to PlayerPowerUp script

    private void Awake()
    {
        _player2Movement = GetComponent<Player2Movement>();
        _currentBulletDamage = normalBulletDamage; // Set default bullet damage
        _playerPowerUp = GetComponent<PlayerPowerUp>();
    }

    void Update()
    {
        if (_fireContinuously)
        {
            float timeSinceLastFire = Time.time - _lastFireTime;

            if (timeSinceLastFire >= _timeBetweenShots)
            {
                FireBullet();
                _lastFireTime = Time.time;
            }
        }
    }

    private void FireBullet()
    {
        Vector2 facingDirection = _player2Movement.GetFacingDirection();
        if (facingDirection == Vector2.zero)
        {
            facingDirection = Vector2.right; // Default direction
        }

        GameObject bullet = Instantiate(_bulletPrefab, _shootingPoint.transform.position, Quaternion.identity);
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();

        // Set the bullet's velocity
        rigidbody.linearVelocity = facingDirection * _bulletSpeed;

        // Rotate the bullet to face the direction of movement
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Set the shooter reference and damage amount in the bullet script
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.shooter = gameObject;
        bulletScript.damageAmount = _currentBulletDamage; // Set the damage amount

        // Notify the PlayerPowerUp script if the bullet damage is powered
        if (_currentBulletDamage == poweredBulletDamage)
        {
            _playerPowerUp.OnPowerShotFired();
        }
    }

    private void OnShoot(InputValue inputValue)
    {
        _fireContinuously = inputValue.isPressed;
    }

    // Method to update bullet damage based on power-up state
    public void SetPoweredBulletDamage(bool isPowered)
    {
        _currentBulletDamage = isPowered ? poweredBulletDamage : normalBulletDamage;
    }

    // Method to fire a powered shot directly from PlayerPowerUp
    public void FirePoweredShot()
    {
        _currentBulletDamage = poweredBulletDamage;
        FireBullet();
        _currentBulletDamage = normalBulletDamage;
    }
}
