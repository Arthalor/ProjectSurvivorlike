using System;
using System.Collections;
using Helper;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [field: SerializeField] public float AttackSpeed { get; private set; }
    [field: SerializeField] public float AttackDamage { get; private set; }
    [field: SerializeField] public float BulletSpeed { get; private set; }
    [field: SerializeField] public float BulletRange { get; private set; }
    [field: SerializeField] public float Projectiles { get; private set; }
    [field: SerializeField] public float BulletPierce { get; private set; }
    [field: SerializeField] public float Health { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }

    [Space]
    [SerializeField] private float invulnerabilityKnockback = default;
    [SerializeField] private Cooldown hitInvulnerabilty = default;

    private void Update()
    {
        hitInvulnerabilty.TickClock(Time.deltaTime);
    }

    public void TakeDamage(int amount)
    {
        if (!hitInvulnerabilty.StartCooldown()) return;
        OnHitKnockback();
        Health -= amount;
        if (Health <= 0) GameManager.Instance.GameOver();
    }

    private void OnHitKnockback()
    {
        Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(transform.position, 3f);

        foreach (Collider2D collider in collidersInRange)
        {
            if (collider.TryGetComponent(out BasicEnemyBehaviour enemy))
            {
                enemy.Knockback(enemy.PlayerDirection(), -1f * invulnerabilityKnockback);
            }
        }
    }
}