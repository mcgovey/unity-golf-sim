using UnityEngine;
using System.Collections;
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
    [SerializeField] private float initDelay = 0.5f; // Delay before initialization to allow other systems to start up

    // Reference to the GameManager to access GameState
    private GameManager gameManager;

    // Currently active golfers
    private List<GolferController> activeGolfers = new List<GolferController>();

    private void Start()
    {
        // Delay initialization to ensure other systems are ready
        StartCoroutine(DelayedInitialization());
    }

    private IEnumerator DelayedInitialization()
    {
        // Wait a moment to ensure other systems have initialized
        yield return new WaitForSeconds(initDelay);

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
            if (gridSystem == null)
            {
                Debug.LogError("GolferSystem could not find GridSystem!");
                yield break;
            }
            else
            {
                Debug.Log("GridSystem found automatically: " + gridSystem.name);
            }
        }

        // Wait for grid to be fully generated
        while (!gridSystem.IsGridGenerated())
        {
            Debug.Log("Waiting for grid to be generated...");
            yield return new WaitForSeconds(0.2f);
        }

        // Get reference to GameManager
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GolferSystem could not find GameManager!");
            yield break;
        }

        Debug.Log($"Starting game with {gameManager.State.BuiltHoles.Count} holes");

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
        foreach (GolferController golfer in activeGolfers.ToArray()) // Use ToArray to avoid modification during iteration
        {
            // Here we could check if the golfer needs a new target
            // For now, we'll just assign targets at spawn time
        }

        // For debugging: Press space to spawn a new golfer
        if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            GolferController golfer = SpawnGolfer();
            FindHoleForGolfer(golfer);
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
        if (golfer == null)
        {
            Debug.LogError("FindHoleForGolfer: Golfer is null!");
            return;
        }

        if (gridSystem == null)
        {
            Debug.LogError("FindHoleForGolfer: GridSystem is null!");
            return;
        }

        if (!gridSystem.IsGridGenerated())
        {
            Debug.LogError("FindHoleForGolfer: Grid is not yet generated!");
            return;
        }

        // Check if there are any holes in the game state
        if (gameManager.State.BuiltHoles.Count > 0)
        {
            Debug.Log($"Found {gameManager.State.BuiltHoles.Count} holes, directing golfer to the first one");

            // For the MVP, just use the first hole
            HoleData targetHole = gameManager.State.BuiltHoles[0];

            Debug.Log($"Target hole position: ({targetHole.Position.x}, {targetHole.Position.y})");

            // Find the world position for this hole
            Vector3 holePosition = gridSystem.GridToWorld(targetHole.Position);

            Debug.Log($"World position for hole: {holePosition}");

            // Tell the golfer to go there
            golfer.SetDestination(holePosition);

            Debug.Log($"Golfer moving to hole at ({targetHole.Position.x}, {targetHole.Position.y})");
        }
        else
        {
            Debug.LogWarning("No holes found for golfer to target! Place a hole first.");
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
