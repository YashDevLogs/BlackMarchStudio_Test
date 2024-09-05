using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IAI
{
    public Transform Player; // Reference to the player unit
    public float moveSpeed = 3f; // Speed of enemy movement

    private Vector2Int currentGridPosition;
    private Vector2Int playerGridPosition;
    private bool isWaiting = false;

    private Pathfinding pathfinding; // Assuming you're using the Pathfinding script from Assignment 3

    private void Start()
    {
        pathfinding = FindObjectOfType<Pathfinding>(); // Find the Pathfinding script in the scene
        currentGridPosition = WorldToGridPosition(transform.position);
    }

    private void Update()
    {
        UpdateAI();
    }

    public void UpdateAI()
    {
        if (!isWaiting)
        {
            playerGridPosition = pathfinding.WorldToGridPosition(Player.position);
            Vector2Int targetPosition = GetAdjacentTileToPlayer();

            if (targetPosition != currentGridPosition)
            {
                MoveTowards(targetPosition);
            }
            else
            {
                isWaiting = true; // Enemy reached the target tile and waits
            }
        }
        else
        {
            // Check if the player has moved, if so, restart the process
            Vector2Int newPlayerGridPosition = pathfinding.WorldToGridPosition(Player.position);
            if (newPlayerGridPosition != playerGridPosition)
            {
                isWaiting = false; // Player has moved, so the enemy should move again
            }
        }
    }

    private Vector2Int GetAdjacentTileToPlayer()
    {
        // Get the adjacent tiles to the player
        List<Vector2Int> adjacentTiles = new List<Vector2Int>
        {
            new Vector2Int(playerGridPosition.x - 1, playerGridPosition.y),
            new Vector2Int(playerGridPosition.x + 1, playerGridPosition.y),
            new Vector2Int(playerGridPosition.x, playerGridPosition.y - 1),
            new Vector2Int(playerGridPosition.x, playerGridPosition.y + 1)
        };

        // Choose a random adjacent tile to move towards
        foreach (var tile in adjacentTiles)
        {
            if (!pathfinding.ObstacleData.IsObstacle(tile.x, tile.y))
            {
                return tile;
            }
        }

        // Default to the player's tile if no valid adjacent tiles are found
        return playerGridPosition;
    }

    private void MoveTowards(Vector2Int targetPosition)
    {
        Vector3 targetWorldPosition = pathfinding.GridToWorldPosition(targetPosition);
        StartCoroutine(MoveCoroutine(targetWorldPosition));
    }

    private IEnumerator MoveCoroutine(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        currentGridPosition = pathfinding.WorldToGridPosition(transform.position);
    }

    private Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        // Convert the world position to grid position
        return pathfinding.WorldToGridPosition(worldPosition);
    }
}
