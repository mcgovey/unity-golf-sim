using UnityEngine;

public class GolferSystem : MonoBehaviour
{
    [Header("Golfer Settings")]
    [SerializeField] private GameObject golferPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Debugging")]
    [SerializeField] private bool spawnOnStart = true;

    private void Start()
    {
        // If no spawn point is set, create one at origin
        if (spawnPoint == null)
        {
            GameObject spawnObj = new GameObject("GolferSpawnPoint");
            spawnPoint = spawnObj.transform;
            spawnPoint.position = new Vector3(-4, 0, -4); // Default position near grid edge
        }

        // Spawn a golfer on start if needed
        if (spawnOnStart)
        {
            SpawnGolfer();
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

        // Log for debugging
        Debug.Log("Spawned new golfer at " + spawnPoint.position);

        return golfer;
    }
}
