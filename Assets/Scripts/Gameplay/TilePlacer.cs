using UnityEngine;
using UnityEngine.InputSystem;

public class TilePlacer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridSystem gridSystem;
    [SerializeField] private Camera mainCamera;

    // Reference to the GameManager to access GameState
    private GameManager gameManager;

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

        // Get reference to GameManager
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        // Check for mouse click (left button)
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Get mouse position
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            // Try to place a hole at the clicked position
            PlaceHoleAtMousePosition(mousePosition);
        }
    }

    private void PlaceHoleAtMousePosition(Vector2 mousePosition)
    {
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
            Tile clickedTile = gridSystem.GetTileAt(gridPosition.x, gridPosition.y);

            // If we found a valid tile
            if (clickedTile != null)
            {
                // Set the tile type to Hole
                clickedTile.SetTileType(Tile.TileType.Hole);

                // Create a new HoleData and add it to GameState
                HoleData newHole = new HoleData(gridPosition);
                gameManager.State.BuiltHoles.Add(newHole);

                // Log for debugging
                Debug.Log($"Placed hole at ({gridPosition.x}, {gridPosition.y})");
                Debug.Log($"Total holes built: {gameManager.State.BuiltHoles.Count}");
            }
        }
    }
}
