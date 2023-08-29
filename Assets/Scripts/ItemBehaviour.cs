using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    [SerializeField] private float flySpeed = default;

    private bool pickedUp = false;
    private float flyTime = 0;

    private void Update()
    {
        if (!pickedUp) return;

        flyTime += Time.deltaTime * flySpeed;
        transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.player.position, flyTime);
    }

    public void PickUp()
    {
        pickedUp = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.Equals(GameManager.Instance.player))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            GameManager.Instance.player.GetComponent<Inventory>().PickUpItem();
            GameManager.Instance.player.GetComponent<PlayerStats>().RecalculateStats();
            Destroy(gameObject, 1);
        }
    }
}