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
    private GameObject[,] tiles;

    private void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        // Initialize tile array
        tiles = new GameObject[gridWidth, gridHeight];

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
                GameObject tile;

                if (tilePrefab != null)
                {
                    // Instantiate from prefab if provided
                    tile = Instantiate(tilePrefab, gridParent);
                }
                else
                {
                    // Create an empty GameObject if no prefab
                    tile = new GameObject($"Tile_{x}_{z}");
                    tile.transform.parent = gridParent;
                }

                // Position the tile
                Vector3 tilePosition = new Vector3(
                    x * tileSize - offsetX,
                    0f,
                    z * tileSize - offsetZ
                );

                tile.transform.position = tilePosition;

                // Store the tile
                tiles[x, z] = tile;

                // Log for debugging
                Debug.Log($"Created tile at ({x}, {z})");
            }
        }

        Debug.Log($"Generated {gridWidth}x{gridHeight} grid");
    }

    // Get a tile at specific coordinates
    public GameObject GetTileAt(int x, int z)
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
