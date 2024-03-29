using System;
using System.Collections.Generic;
using Helper;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [field: SerializeField] public Stat AttackSpeed { get; private set; }
    [field: SerializeField] public Stat AttackDamage { get; private set; }
    [field: SerializeField] public Stat MaxAmmo { get; private set; }
    [field: SerializeField] public Stat ReloadTime { get; private set; }
    [field: SerializeField] public Stat BulletSpeed { get; private set; }
    [field: SerializeField] public Stat BulletRange { get; private set; }
    [field: SerializeField] public Stat Projectiles { get; private set; }
    [field: SerializeField] public Stat BulletPierce { get; private set; }
    [field: SerializeField] public TemporaryStat Health { get; private set; }
    [field: SerializeField] public Stat MovementSpeed { get; private set; }

    [Space]
    [SerializeField] private float invulnerabilityKnockback = default;
    [SerializeField] private Cooldown hitInvulnerabilty = default;
    [Space]
    [SerializeField] private AudioClip hurtSound = default;
    [SerializeField] private AudioSource hurtSource = default;
    [Space]
    [SerializeField] private InGameUI inGameUI = default;
    [Space(10)]
    public List<Stat> statList;

    private PlayerManager playerManager = default;

    private void Start()
    {
        playerManager = GameManager.Instance.playerManager;
        RecalculateStats();
        statList = ListStats();
    }

    private List<Stat> ListStats() 
    {
        List<Stat> returnList = new List<Stat>
        {
            Health,
            AttackDamage,
            AttackSpeed,
            MaxAmmo,
            ReloadTime,
            BulletSpeed,
            BulletRange,
            MovementSpeed,
        };
        return returnList;
    } 

    public void RecalculateStats() 
    {
        Inventory inv = playerManager.playerInventory;
        AttackSpeed.CalculateStat(inv.items);
        AttackDamage.CalculateStat(inv.items);
        BulletSpeed.CalculateStat(inv.items);
        BulletRange.CalculateStat(inv.items);
        Projectiles.CalculateStat(inv.items);
        BulletPierce.CalculateStat(inv.items);
        int oldMaxHealth = (int)Health.CalculatedStat;
        Health.CalculateStat(inv.items);
        if (Health.CalculatedStat > oldMaxHealth)
        {
            int delta = (int)Health.CalculatedStat - oldMaxHealth;
            Health.CurrentStat = Mathf.Min(Health.CurrentStat + delta, Health.CalculatedStat);
        }
        MovementSpeed.CalculateStat(inv.items);
        MaxAmmo.CalculateStat(inv.items);
        ReloadTime.CalculateStat(inv.items);
        if (GameManager.Instance.player == null) return;
        GameManager.Instance.playerManager.playerControls.RecalculateAttackSpeed();
        UpdateHealthBar();
    }

    private void Update()
    {
        hitInvulnerabilty.TickClock(Time.deltaTime);
    }

    public void TakeDamage(int amount)
    {
        hurtSource.clip = hurtSound;
        hurtSource.Play();
        if (!hitInvulnerabilty.StartCooldown()) return;
        OnHitKnockback();
        Health.CurrentStat -= amount;
        UpdateHealthBar();
        if (Health.CurrentStat <= 0) Die();
    }

    private void UpdateHealthBar() 
    {
        inGameUI.UpdateHealthBar(Health.CalculatedStat, Health.CurrentStat);
    }

    private void Die()
    {
        GameManager.Instance.gamePlayManager.GameLost();
        playerManager.spriteRenderer.enabled = false;
        playerManager.playerControls.Die();
        playerManager.rb.bodyType = RigidbodyType2D.Static;
        playerManager.coll2d.enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
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