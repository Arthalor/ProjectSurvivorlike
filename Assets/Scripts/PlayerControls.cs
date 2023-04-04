using Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler inputHandler = default;
    [SerializeField] private int health = default;
    [SerializeField] private float movementSpeed = default;
    [SerializeField] private float gunDistance = default;
    [SerializeField] private float attackDelay = default;
    private Timer attackTimer;

    [SerializeField] private GameObject bulletPrefab = default;
    [SerializeField] private Transform gunBarrel = default;

    private Rigidbody2D rb = default;
    private PlayerInput input = default;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackTimer = new Timer(attackDelay);
    }

    private void FixedUpdate()
    {
        rb.velocity = movementSpeed * new Vector3(input.movement.x, input.movement.y, 0).normalized;
    }

    void Update()
    {
        input = inputHandler.Input();

        GunBehaviour();

        attackTimer.TickTimer(Time.deltaTime);
        if (input.shooting && attackTimer.TimerFinished()) 
        {
            Shoot();
        }
    }

    void Shoot() 
    {
        attackTimer.ResetTimer();
        Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);
    }

    private void GunBehaviour() 
    {
        Vector3 gunDirection = input.worldMousePosition - new Vector2(transform.position.x, transform.position.y);
        gunBarrel.position = transform.position + gunDirection.normalized * gunDistance;
        float angle = (Mathf.Rad2Deg * Mathf.Atan2(gunDirection.y, gunDirection.x));
        gunBarrel.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void TakeDamage(int amount) 
    {
        OnHitKnockback();
        health -= amount;
        if (health <= 0) GameManager.Instance.GameOver();
    }

    private void OnHitKnockback() 
    {
        Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(transform.position, 3f);

        foreach (Collider2D collider in collidersInRange)
        {
            if (collider.TryGetComponent(out BasicEnemyBehaviour enemy))
            {
                enemy.Knockback();
            }
        }
    }
}