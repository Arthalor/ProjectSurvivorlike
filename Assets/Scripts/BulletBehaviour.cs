using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb = default;
    [SerializeField] private float bulletSpeed = default;
    [SerializeField] private float lifeTime = default;

    private void Start()
    {
        rb.velocity = transform.right * bulletSpeed;
        Destroy(gameObject, lifeTime);
    }
}