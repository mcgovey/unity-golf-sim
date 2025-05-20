using UnityEngine;
using UnityEngine.InputSystem;

public class TileHighlighter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridSystem gridSystem;
    [SerializeField] private Camera mainCamera;

    [Header("Highlight Settings")]
    [SerializeField] private Color highlightColor = new Color(1f, 1f, 1f, 0.5f);

    // Currently highlighted tile
    private Tile currentHighlightedTile;
    // Original color of the highlighted tile
    private Color originalColor;
    // Material of the highlighted tile
    private Material highlightedMaterial;

    private void Start()
    {
        // If mainCamera is not set, try to find the main camera
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // If gridSystem is not set, try to find it in the scene
        if (gridSystem == null)
        {
            gridSystem = FindObjectOfType<GridSystem>();
        }
    }

    private void Update()
    {
        // Get mouse position using the new Input System
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        // Cast a ray from the mouse position
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // Check if the ray hits something
        if (Physics.Raycast(ray, out hit))
        {
            // Get the position in world space
            Vector3 hitPoint = hit.point;

            // Convert to grid coordinates
            Vector2Int gridPosition = gridSystem.WorldToGrid(hitPoint);

            // Get the tile at those coordinates
            Tile hoveredTile = gridSystem.GetTileAt(gridPosition.x, gridPosition.y);

            // If we found a valid tile
            if (hoveredTile != null)
            {
                // If this is a different tile than the one we were highlighting
                if (hoveredTile != currentHighlightedTile)
                {
                    // Restore the original color of the previously highlighted tile
                    ResetHighlight();

                    // Highlight the new tile
                    HighlightTile(hoveredTile);
                }
            }
            else
            {
                // No valid tile found, reset highlight
                ResetHighlight();
            }
        }
        else
        {
            // No hit at all, reset highlight
            ResetHighlight();
        }
    }

    // Apply highlight to a tile
    private void HighlightTile(Tile tile)
    {
        // Get the tile's renderer
        MeshRenderer renderer = tile.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            // Store the tile
            currentHighlightedTile = tile;
            highlightedMaterial = renderer.material;

            // Store original color
            originalColor = highlightedMaterial.color;

            // Set highlight color
            highlightedMaterial.color = highlightColor;

            // Log for debugging
            Debug.Log($"Highlighting tile at {tile.GetGridPosition()}");
        }
    }

    // Reset the highlight
    private void ResetHighlight()
    {
        if (currentHighlightedTile != null && highlightedMaterial != null)
        {
            // Restore original color
            highlightedMaterial.color = originalColor;

            // Clear references
            currentHighlightedTile = null;
            highlightedMaterial = null;
        }
    }
}
