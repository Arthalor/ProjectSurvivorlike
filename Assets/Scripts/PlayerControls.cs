using Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerControls : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler inputHandler = default;
    [SerializeField] private PlayerStats playerStats = default;

    [SerializeField] private GameObject bulletPrefab = default;
    [SerializeField] private Transform gunBarrel = default;
    [SerializeField] private float gunDistance = default;

    private Timer attackTimer;

    private Rigidbody2D rb;
    private PlayerInput input;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackTimer = new Timer(1 / playerStats.AttackSpeed);
    }

    private void FixedUpdate()
    {
        rb.velocity = playerStats.MovementSpeed * new Vector3(input.movement.x, input.movement.y, 0).normalized;
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
        GameObject bullet = Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);
        BulletBehaviour bulletBehaviour = bullet.GetComponent<BulletBehaviour>();
        bulletBehaviour.SetStats(playerStats.AttackDamage, playerStats.BulletSpeed, playerStats.BulletRange);
    }

    private void GunBehaviour() 
    {
        Vector3 gunDirection = input.worldMousePosition - new Vector2(transform.position.x, transform.position.y);
        gunBarrel.position = transform.position + gunDirection.normalized * gunDistance;
        float angle = (Mathf.Rad2Deg * Mathf.Atan2(gunDirection.y, gunDirection.x));
        gunBarrel.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}