using UnityEngine;

public class Tile : MonoBehaviour
{
    // Grid position of this tile
    private Vector2Int gridPosition;

    // Current tile type (empty, hole, etc.)
    private TileType tileType = TileType.Empty;

    // Material reference for color changes
    private MeshRenderer meshRenderer;

    // Define tile types
    public enum TileType
    {
        Empty,
        Hole,
        Tee,
        Fairway,
        Rough,
        Bunker,
        Water
    }

    private void Awake()
    {
        // Get the MeshRenderer component
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Initialize the tile with its grid position
    public void Initialize(Vector2Int position)
    {
        gridPosition = position;
        name = $"Tile_{position.x}_{position.y}";
    }

    // Get the current grid position
    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    // Get the current tile type
    public TileType GetTileType()
    {
        return tileType;
    }

    // Set the tile type and update visuals
    public void SetTileType(TileType newType)
    {
        tileType = newType;
        UpdateVisuals();
    }

    // Update the tile's appearance based on its type
    private void UpdateVisuals()
    {
        if (meshRenderer == null) return;

        // Change color based on tile type
        Color tileColor = Color.white; // Default empty color

        switch (tileType)
        {
            case TileType.Empty:
                tileColor = Color.white;
                break;
            case TileType.Hole:
                tileColor = Color.red;
                break;
            case TileType.Tee:
                tileColor = Color.blue;
                break;
            case TileType.Fairway:
                tileColor = Color.green;
                break;
            case TileType.Rough:
                tileColor = new Color(0.5f, 0.8f, 0.2f); // Darker green
                break;
            case TileType.Bunker:
                tileColor = Color.yellow;
                break;
            case TileType.Water:
                tileColor = Color.cyan;
                break;
        }

        // Apply the color to the material
        Material material = meshRenderer.material;
        material.color = tileColor;
    }
}
