using Helper;
using static Helper.SpriteRendererFunctions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerControls : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler = default;
    [SerializeField] private PlayerStats playerStats = default;
    [Space]
    [SerializeField] private GameObject bulletPrefab = default;
    [SerializeField] private Transform gunBarrel = default;
    [SerializeField] private float gunDistance = default;
    [SerializeField] private float gunSortingHeight = default;
    [Space]
    [SerializeField] private AudioClip shootSound = default;
    [SerializeField] private AudioSource shootSource = default;

    private Timer attackTimer;
    private int magLoad;
    private Timer reloadingTimer;
    private bool reloading = false;
    private bool isDead = false;

    private Rigidbody2D rb;
    private PlayerInput input;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitializeStats();
    }

    void InitializeStats() 
    {
        if (playerStats.AttackSpeed.CalculatedStat <= 0)
        {
            attackTimer = new Timer(1 / playerStats.AttackSpeed.BaseStat);
        }
        reloadingTimer = new Timer(playerStats.ReloadTime.BaseStat, playerStats.ReloadTime.BaseStat);
        magLoad = Mathf.RoundToInt(playerStats.MaxAmmo.BaseStat);
    }

    public void RecalculateAttackSpeed()
    {
        string name = playerStats.AttackSpeed.Name;
        float bStat = playerStats.AttackSpeed.BaseStat;
        float cStat = playerStats.AttackSpeed.CalculatedStat;
        attackTimer = new Timer(1 / playerStats.AttackSpeed.CalculatedStat);
        reloadingTimer = new Timer(playerStats.ReloadTime.CalculatedStat);
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        rb.velocity = playerStats.MovementSpeed.CalculatedStat * new Vector3(input.movement.x, input.movement.y, 0).normalized;
    }

    void Update()
    {
        if (isDead) return;
        MoveGun();
        FlipSpriteToMovement(spriteRenderer, rb.velocityX);
        input = inputHandler.Input();

        attackTimer.Tick(Time.deltaTime);
        reloadingTimer.Tick(Time.deltaTime);
        GameManager.Instance.gamePlayManager.inGameUI.UpdateReloadUI(reloading, reloadingTimer.Progress());

        if (reloadingTimer.Finished() && reloading)
        {
            Reload();
        }

        if (input.shoot && attackTimer.Finished() && reloadingTimer.Finished()) 
        {
            if (magLoad > 0)
            {
                Shoot();
            }
            else
            {
                StartReload();
            }
        }

        if (input.reload) 
        {
            StartReload();
        }
    }

    void Shoot() 
    {
        magLoad--;
        shootSource.clip = shootSound;
        shootSource.Play();
        attackTimer.Reset();
        SpawnBullet();
    }

    void StartReload()
    {
        if (reloading) return;
        reloadingTimer.Reset();
        reloading = true;
    }

    void Reload()
    {
        magLoad = Mathf.RoundToInt(playerStats.MaxAmmo.CalculatedStat);
        reloading = false;
    }

    void SpawnBullet() 
    {
        GameObject bullet = Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);
        BulletBehaviour bulletBehaviour = bullet.GetComponent<BulletBehaviour>();
        bulletBehaviour.SetStats(playerStats.AttackDamage.CalculatedStat, playerStats.BulletSpeed.CalculatedStat, playerStats.BulletRange.CalculatedStat);
    }

    private void MoveGun() 
    {
        Vector3 gunDirection = input.worldMousePosition - new Vector2(transform.position.x, transform.position.y);
        gunBarrel.position = transform.position + gunDirection.normalized * gunDistance;
        float angle = (Mathf.Rad2Deg * Mathf.Atan2(gunDirection.y, gunDirection.x));
        gunBarrel.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        gunBarrel.GetComponent<SpriteRenderer>().flipY = angle >= 90 || angle <= -90;
        gunBarrel.GetComponent<SpriteRenderer>().sortingOrder = gunBarrel.localPosition.y <= gunSortingHeight ? 2 : 0;
    }

    public void Die() 
    {
        isDead = true;
    }
}