using System;
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
    [Space]
    [SerializeField] private AudioClip hurtSound = default;
    [SerializeField] private AudioSource hurtSource = default;
    [Space]
    [SerializeField] private int experience = 0;
    [Space]
    [SerializeField] private InGameUI inGameUI = default;
    [Space(10)]
    public List<Stat> statList;

    private void Start()
    {
        RecalculateStats();
        statList = ListStats();
    }

    private List<Stat> ListStats() 
    {
        List<Stat> returnList = new List<Stat>();
        returnList.Add(Health);
        returnList.Add(AttackDamage);
        returnList.Add(AttackSpeed);
        returnList.Add(BulletSpeed);
        returnList.Add(BulletRange);
        returnList.Add(MovementSpeed);
        return returnList;
    } 

    public void RecalculateStats() 
    {
        Inventory inv = GetComponent<Inventory>();
        AttackSpeed.CalculateStat(inv.items);
        AttackDamage.CalculateStat(inv.items);
        BulletSpeed.CalculateStat(inv.items);
        BulletRange.CalculateStat(inv.items);
        Projectiles.CalculateStat(inv.items);
        BulletPierce.CalculateStat(inv.items);
        Health.CalculateStat(inv.items);
        MovementSpeed.CalculateStat(inv.items);
        if (GameManager.Instance.player == null) return;
        GameManager.Instance.player.GetComponent<PlayerControls>().RecalculateAttackSpeed();
    }

    private void Update()
    {
        hitInvulnerabilty.TickClock(Time.deltaTime);
        AttractExperience();
    }

    public void TakeDamage(int amount)
    {
        hurtSource.clip = hurtSound;
        hurtSource.Play();
        if (!hitInvulnerabilty.StartCooldown()) return;
        OnHitKnockback();
        Health.CurrentStat -= amount;
        inGameUI.UpdateHealthBar(Health.BaseStat, Health.CurrentStat);
        if (Health.CurrentStat <= 0) Die();
    }

    private void Die()
    {
        GameManager.Instance.gamePlayManager.GameLost();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PlayerControls>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<CircleCollider2D>().enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }

    public int Level() 
    {
        //Formula for Level is derived from Xp needed ((3xp + Level*xp) per Level)
        int i = (int)Mathf.Max((-2.5f + Mathf.Sqrt(6.25f + 2 * experience)) / 1, (-2.5f - Mathf.Sqrt(6.25f + 2 * experience)) / 1);
        return i;
    }

    public int XPtoNextLevel() 
    {
        return 3 + Level();
    }

    public int CumulativeXPtoCurrentLevel() 
    {
        if (Level() <= 0) return 0;
        int returnValue = 0;
        for (int i = 0; i < Level(); i++) 
        {
            returnValue += 3 + i;
        }
        return returnValue;
    }

    public void AttractExperience()
    {
        Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (Collider2D collider in collidersInRange) 
        {
            if (collider.TryGetComponent(out ExperienceBehaviour expBehaviour)) 
            {
                expBehaviour.PickUp();
            }
        }
    }

    public void PickUpExperience(int amount) 
    {
        experience += amount;
        inGameUI.UpdateExpBar(XPtoNextLevel(), experience - CumulativeXPtoCurrentLevel());
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