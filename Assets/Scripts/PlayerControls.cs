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
    [SerializeField] private Timer attackTimer = default;

    [SerializeField] private GameObject bulletPrefab = default;
    [SerializeField] private Transform gunBarrel = default;

    private PlayerInput input = default;

    void Update()
    {
        input = inputHandler.Input();

        transform.position += movementSpeed * Time.deltaTime * new Vector3(input.movement.x, input.movement.y, 0).normalized;
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
        health -= amount;
        if (health <= 0) GameManager.Instance.GameOver();
    }
}