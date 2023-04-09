using System;
using System.Collections;
using System.Collections.Generic;
using Helper;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [field: SerializeField] public Stat AttackSpeed { get; private set; }
    [field: SerializeField] public Stat AttackDamage { get; private set; }
    [field: SerializeField] public Stat BulletSpeed { get; private set; }
    [field: SerializeField] public Stat BulletRange { get; private set; }
    [field: SerializeField] public Stat Projectiles { get; private set; }
    [field: SerializeField] public Stat BulletPierce { get; private set; }
    [field: SerializeField] public TemporaryStat Health { get; private set; }
    [field: SerializeField] public Stat MovementSpeed { get; private set; }

    [Space]
    [SerializeField] private float invulnerabilityKnockback = default;
    [SerializeField] private Cooldown hitInvulnerabilty = default;

    private void Start()
    {
        Health.InitializeCurrent();
        Inventory inv = GetComponent<Inventory>();
        RecalculateStats(inv.items);
    }

    public void RecalculateStats() 
    {
        AttackSpeed.CalculateStat();
        AttackDamage.CalculateStat();
        BulletSpeed.CalculateStat();
        BulletRange.CalculateStat();
        Projectiles.CalculateStat();
        BulletPierce.CalculateStat();
        Health.CalculateStat();
        MovementSpeed.CalculateStat();
    }

    public void RecalculateStats(List<Item> items) 
    {
        AttackSpeed.CalculateStat(items);
        AttackDamage.CalculateStat(items);
        BulletSpeed.CalculateStat(items);
        BulletRange.CalculateStat(items);
        Projectiles.CalculateStat(items);
        BulletPierce.CalculateStat(items);
        Health.CalculateStat(items);
        MovementSpeed.CalculateStat(items);
    }

    private void Update()
    {
        hitInvulnerabilty.TickClock(Time.deltaTime);
    }

    public void TakeDamage(int amount)
    {
        if (!hitInvulnerabilty.StartCooldown()) return;
        OnHitKnockback();
        Health.CurrentStat -= amount;
        if (Health.CurrentStat <= 0) GameManager.Instance.GameOver();
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