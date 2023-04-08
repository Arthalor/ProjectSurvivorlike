using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    [SerializeField] private Vector2 tileSize = default;
    [SerializeField] private GameObject tile = default;
    [SerializeField] private Transform tileParent = default;

    private Transform player;

    private List<GameObject> instantiatedTiles = new List<GameObject>();

    private void Start()
    {
        player = transform;
    }

    private void Update()
    {
        DespawnGrid();
        SpawnGrid();
    }

    void SpawnGrid() 
    {
        Vector2 roundedPlayerPosition = 
            new(Mathf.Round(player.position.x / tileSize.x) * tileSize.x, Mathf.Round(player.position.y / tileSize.y) * tileSize.y);
        Vector2 roundedOffsetPlayerPosition = new(roundedPlayerPosition.x - tileSize.x, roundedPlayerPosition.y - tileSize.y);
        for (int i = 0; i < 9; i++) 
        {
            int xCoordinate = (int)(roundedOffsetPlayerPosition.x + tileSize.x * (i / 3));
            int yCoordinate = (int)(roundedOffsetPlayerPosition.y + tileSize.y * (i % 3));
            Vector2 tilePosition = new(xCoordinate, yCoordinate);
            
            GameObject tileForList = Instantiate(tile, tilePosition, Quaternion.identity, tileParent);
            instantiatedTiles.Add(tileForList);
        }
    }

    void DespawnGrid() 
    {
        if (instantiatedTiles.Count <= 0) return;
        foreach (GameObject instantiatedTile in instantiatedTiles) 
        {
            Destroy(instantiatedTile);
        }
    }
}