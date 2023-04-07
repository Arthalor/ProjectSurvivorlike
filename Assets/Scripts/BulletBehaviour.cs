using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    public void SetStats(float _damage, float _bulletSpeed, float _bulletRange) 
    {
        damage = _damage;
        bulletSpeed = _bulletSpeed;
        lifeTime = _bulletRange / bulletSpeed;
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