using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb = default;
    [SerializeField] private float damage = default;
    [SerializeField] private float bulletSpeed = default;
    [SerializeField] private float lifeTime = default;

    private void Start()
    {
        rb.velocity = transform.right * bulletSpeed;
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out BasicEnemyBehaviour enemy))
        {
            enemy.TakeDamage(damage);
            //collision.otherCollider.enabled = false;
            Destroy(gameObject);
        }
    }
}