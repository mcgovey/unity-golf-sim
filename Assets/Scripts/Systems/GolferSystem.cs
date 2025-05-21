using UnityEngine;
using System.Collections.Generic;

public class GolferSystem : MonoBehaviour
{
    [Header("Golfer Settings")]
    [SerializeField] private GameObject golferPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("References")]
    [SerializeField] private GridSystem gridSystem;

    [Header("Debugging")]
    [SerializeField] private bool spawnOnStart = true;

    // Reference to the GameManager to access GameState
    private GameManager gameManager;

    // Currently active golfers
    private List<GolferController> activeGolfers = new List<GolferController>();

    private void Start()
    {
        // If no spawn point is set, create one at origin
        if (spawnPoint == null)
        {
            GameObject spawnObj = new GameObject("GolferSpawnPoint");
            spawnPoint = spawnObj.transform;
            spawnPoint.position = new Vector3(-4, 0, -4); // Default position near grid edge
        }

        // If gridSystem is not set, try to find it in the scene
        if (gridSystem == null)
        {
            gridSystem = FindObjectOfType<GridSystem>();
        }

        // Get reference to GameManager
        gameManager = GameManager.Instance;

        // Spawn a golfer on start if needed
        if (spawnOnStart)
        {
            GolferController golfer = SpawnGolfer();

            // Find a hole to target if there are any
            FindHoleForGolfer(golfer);
        }
    }

    private void Update()
    {
        // Check if any golfers need to be assigned targets
        foreach (GolferController golfer in activeGolfers)
        {
            // Here we could check if the golfer needs a new target
            // For now, we'll just assign targets at spawn time
        }
    }

    // Spawn a new golfer at the spawn point
    public GolferController SpawnGolfer()
    {
        // Check if prefab exists
        if (golferPrefab == null)
        {
            Debug.LogError("No golfer prefab assigned to GolferSystem!");
            return null;
        }

        // Instantiate the golfer
        GameObject golferObject = Instantiate(golferPrefab, spawnPoint.position, Quaternion.identity);
        GolferController golfer = golferObject.GetComponent<GolferController>();

        // Add to active golfers list
        activeGolfers.Add(golfer);

        // Log for debugging
        Debug.Log("Spawned new golfer at " + spawnPoint.position);

        return golfer;
    }

    // Find a hole for the golfer to move to
    private void FindHoleForGolfer(GolferController golfer)
    {
        if (golfer == null) return;

        // Check if there are any holes in the game state
        if (gameManager.State.BuiltHoles.Count > 0)
        {
            // For the MVP, just use the first hole
            HoleData targetHole = gameManager.State.BuiltHoles[0];

            // Find the world position for this hole
            Vector3 holePosition = gridSystem.GridToWorld(targetHole.Position);

            // Tell the golfer to go there
            golfer.SetDestination(holePosition);

            Debug.Log($"Golfer moving to hole at ({targetHole.Position.x}, {targetHole.Position.y})");
        }
        else
        {
            Debug.Log("No holes found for golfer to target");
        }
    }

    // Remove golfer from the active list (called when a golfer is destroyed)
    public void RemoveGolfer(GolferController golfer)
    {
        if (activeGolfers.Contains(golfer))
        {
            activeGolfers.Remove(golfer);
            Debug.Log("Golfer removed from active list");
        }
    }
}
