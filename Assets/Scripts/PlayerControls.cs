using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler inputHandler = default;
    [SerializeField] private float movementSpeed = default;

    [SerializeField] private GameObject bulletPrefab = default;
    [SerializeField] private Transform gunBarrel = default;

    private PlayerInput input = default;

    void Update()
    {
        input = inputHandler.Input();

        transform.position += movementSpeed * Time.deltaTime * new Vector3(input.movement.x, input.movement.y, 0).normalized;

        if (input.shooting && ReadyToShoot()) 
        {
            Shoot();
        }
    }

    private bool ReadyToShoot() 
    {
        return true;
    }

    void Shoot() 
    {
        Instantiate(bulletPrefab, gunBarrel.position, Quaternion.identity);
    }
}