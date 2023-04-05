using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Helper.VectorFunctions;

public class BasicEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float health = default;
    [SerializeField] private float moveSpeed = default;
    [SerializeField] private float bounceForce = default;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 currentVelocity;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        AccelerateTowardsPlayer();
    }

    protected void AccelerateTowardsPlayer()
    {
        Vector2 targetVelocity = PlayerDirection() * moveSpeed;
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, 0.5f);
    }

    public Vector2 PlayerDirection()
    {
        return DirectionAtoB(transform.position, player.position);
    }

    public void Knockback(Vector2 direction, float bounceFactor)
    {
        rb.AddForce(direction * bounceFactor,ForceMode2D.Impulse);
    }

    public void TakeDamage(float damage) 
    {
        health -= damage;
        if (health <= 0) Die();
    }

    private void Die() 
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out PlayerControls player))
        {
            player.TakeDamage(1);
        }
        else Knockback(DirectionAtoB(collision.transform.position, transform.position), bounceForce);
    }
}