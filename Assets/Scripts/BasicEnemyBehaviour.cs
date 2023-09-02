using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Helper.VectorFunctions;
using static Helper.SpriteRendererFunctions;

public class BasicEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float health = default;
    [SerializeField] private float moveSpeed = default;
    [SerializeField] private float bounceForce = default;
    [Space]
    [SerializeField] private GameObject damagePopUp = default;
    [SerializeField] private GameObject experience = default;
    [SerializeField] private GameObject itemObject = default;
    
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 currentVelocity;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        FlipSpriteToMovement(spriteRenderer, rb.velocityX);
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
        DamagePopUp(damage);
        if (health <= 0) Die();
    }

    private void DamagePopUp(float damage) 
    {
        Vector2 spawnPoint = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle;
        GameObject popUp = Instantiate(damagePopUp, spawnPoint, Quaternion.identity);
        popUp.GetComponent<DamageNumbersBehaviour>().SetText("" + damage);
    }

    private void Die() 
    {
        Vector2 spawnPoint = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle * 0.5f;
        float random = Random.Range(0f, 1f);
        if (random > 0.33f)
        {
            GameObject xp = Instantiate(experience, spawnPoint, Quaternion.identity);
        }
        else
        {
            GameObject item = Instantiate(itemObject, spawnPoint, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out PlayerStats player))
        {
            player.TakeDamage(1);
        }
        else Knockback(DirectionAtoB(collision.transform.position, transform.position), bounceForce);
    }
}