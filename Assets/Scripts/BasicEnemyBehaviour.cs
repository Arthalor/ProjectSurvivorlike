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

    protected Vector2 PlayerDirection()
    {
        return DirectionAtoB(transform.position, player.position);
    }

    protected void Knockback(Vector2 direction)
    {
        rb.AddForce(direction * bounceForce,ForceMode2D.Impulse);
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

    public void Knockback() 
    {      
        Knockback(PlayerDirection() * -10);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Knockback(DirectionAtoB(collision.transform.position, transform.position));
        if (collision.collider.TryGetComponent(out PlayerControls player))
        {
            player.TakeDamage(1);
        }
    }
}