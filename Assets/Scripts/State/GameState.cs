using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    // Player's current cash amount
    public int PlayerCash { get; set; }

    // List of holes that have been built
    public List<HoleData> BuiltHoles { get; set; }

    // Constructor with default values
    public GameState()
    {
        PlayerCash = 1000; // Starting cash
        BuiltHoles = new List<HoleData>();
    }

    // For debugging
    public override string ToString()
    {
        return $"GameState: Cash={PlayerCash}, Holes={BuiltHoles.Count}";
    }
}
