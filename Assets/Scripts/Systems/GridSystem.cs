using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private float tileSize = 1f;

    [Header("References")]
    [SerializeField] private GameObject tilePrefab;

    // Store created tiles
    private Tile[,] tiles;

    private void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        // Initialize tile array
        tiles = new Tile[gridWidth, gridHeight];

        // Calculate offset to center the grid
        float offsetX = (gridWidth - 1) * tileSize * 0.5f;
        float offsetZ = (gridHeight - 1) * tileSize * 0.5f;

        // Create parent object for organization
        Transform gridParent = transform;

        // Generate tiles
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                // Create tile GameObject
                GameObject tileObject;
                Tile tileComponent;

                if (tilePrefab != null)
                {
                    // Instantiate from prefab if provided
                    tileObject = Instantiate(tilePrefab, gridParent);
                    tileComponent = tileObject.GetComponent<Tile>();

                    // If the prefab doesn't have a Tile component, add one
                    if (tileComponent == null)
                    {
                        tileComponent = tileObject.AddComponent<Tile>();
                    }
                }
                else
                {
                    // Create a primitive cube if no prefab
                    tileObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    tileObject.transform.parent = gridParent;
                    tileObject.transform.localScale = new Vector3(0.9f, 0.1f, 0.9f); // Flat cube with small gaps
                    tileComponent = tileObject.AddComponent<Tile>();
                }

                // Position the tile
                Vector3 tilePosition = new Vector3(
                    x * tileSize - offsetX,
                    0f,
                    z * tileSize - offsetZ
                );

                tileObject.transform.position = tilePosition;

                // Initialize the tile
                Vector2Int gridPosition = new Vector2Int(x, z);
                tileComponent.Initialize(gridPosition);

                // Store the tile
                tiles[x, z] = tileComponent;

                // Log for debugging
                Debug.Log($"Created tile at ({x}, {z})");
            }
        }

        Debug.Log($"Generated {gridWidth}x{gridHeight} grid");
    }

    // Get a tile at specific coordinates
    public Tile GetTileAt(int x, int z)
    {
        if (x >= 0 && x < gridWidth && z >= 0 && z < gridHeight)
        {
            return tiles[x, z];
        }
        return null;
    }

    // Convert world position to grid coordinates
    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        float offsetX = (gridWidth - 1) * tileSize * 0.5f;
        float offsetZ = (gridHeight - 1) * tileSize * 0.5f;

        int x = Mathf.RoundToInt((worldPosition.x + offsetX) / tileSize);
        int z = Mathf.RoundToInt((worldPosition.z + offsetZ) / tileSize);

        // Clamp to valid grid range
        x = Mathf.Clamp(x, 0, gridWidth - 1);
        z = Mathf.Clamp(z, 0, gridHeight - 1);

        return new Vector2Int(x, z);
    }

    // Convert grid coordinates to world position
    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        float offsetX = (gridWidth - 1) * tileSize * 0.5f;
        float offsetZ = (gridHeight - 1) * tileSize * 0.5f;

        float x = gridPosition.x * tileSize - offsetX;
        float z = gridPosition.y * tileSize - offsetZ;

        return new Vector3(x, 0f, z);
    }
}
