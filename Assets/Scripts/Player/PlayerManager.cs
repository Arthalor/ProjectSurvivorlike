using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public SpriteRenderer spriteRenderer = default;
    public Rigidbody2D rb = default;
    public Collider2D coll2d = default;
    [Space(20)]
    public PlayerControls playerControls = default;
    public PlayerStats playerStats = default;
    public Inventory playerInventory = default;
    public PlayerLeveling playerLeveling = default;
}