using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float health = default;
    [SerializeField] private float moveSpeed = default;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer() 
    {
        transform.position += (player.position - transform.position).normalized * moveSpeed * Time.deltaTime;
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
}