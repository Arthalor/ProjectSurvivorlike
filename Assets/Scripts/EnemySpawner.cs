using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Vector2 screenCorner = default;
    [SerializeField] private float minSpawnDelay = default;
    [SerializeField] private float maxSpawnDelay = default;
    [Space]
    [SerializeField] private Transform player = default;
    [SerializeField] private GameObject enemy = default;
    
    private float screenRadius = default;
    private float time = default;
    private float calculatedSpawnDelay = 2f;

    private void Start()
    {
        screenRadius = screenCorner.magnitude + 1;
    }

    void Update()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 randomPointOnEnemySpawnCircle = new Vector2(player.position.x, player.position.y) + randomDirection * screenRadius;

        time += Time.deltaTime;
        if (time >= calculatedSpawnDelay)
        {
            Instantiate(enemy, randomPointOnEnemySpawnCircle, Quaternion.identity);
            calculatedSpawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            time = 0;
        }
    }
}
