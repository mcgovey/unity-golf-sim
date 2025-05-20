using UnityEngine;

public class HoleData
{
    // Position of the hole on the grid
    public Vector2Int Position { get; set; }

    // Par value for the hole
    public int Par { get; set; }

    // Constructor with parameters
    public HoleData(Vector2Int position, int par = 3)
    {
        Position = position;
        Par = par;
    }

    // For debugging
    public override string ToString()
    {
        return $"Hole at ({Position.x}, {Position.y}), Par: {Par}";
    }
}
