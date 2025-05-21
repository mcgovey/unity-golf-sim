using UnityEngine;
using System.Collections;

public class GolferController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Gameplay")]
    [SerializeField] private int score = 0;

    // Current target position
    private Vector3 targetPosition;
    // Whether the golfer is currently moving
    private bool isMoving = false;

    // Reference to the GolferSystem
    private GolferSystem golferSystem;

    private void Start()
    {
        // Set initial appearance
        SetupVisuals();

        // Find the GolferSystem
        golferSystem = FindObjectOfType<GolferSystem>();
    }

    private void SetupVisuals()
    {
        // If there's no visual representation, create a simple one
        if (GetComponent<MeshRenderer>() == null && GetComponent<SpriteRenderer>() == null)
        {
            // Create a simple representation (a tall capsule)
            GameObject visualObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            visualObject.transform.parent = transform;
            visualObject.transform.localPosition = Vector3.zero;
            visualObject.transform.localScale = new Vector3(0.5f, 0.75f, 0.5f);

            // Add a material with a distinct color
            MeshRenderer renderer = visualObject.GetComponent<MeshRenderer>();
            Material material = new Material(Shader.Find("Standard"));
            material.color = new Color(0f, 0.3f, 0.7f); // Blue color
            renderer.material = material;
        }
    }

    // Set the target position for the golfer to move to
    public void SetDestination(Vector3 destination)
    {
        targetPosition = destination;

        // Adjust height to match golfer's position (y-axis)
        targetPosition.y = transform.position.y;

        // Start moving if not already
        if (!isMoving)
        {
            StartCoroutine(MoveToDestination());
        }
    }

    // Move to the current target position
    private IEnumerator MoveToDestination()
    {
        isMoving = true;

        // Calculate distance to target
        float distance = Vector3.Distance(transform.position, targetPosition);

        while (distance > 0.1f)
        {
            // Move towards target
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            // Look towards movement direction
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                transform.forward = direction;
            }

            // Recalculate distance
            distance = Vector3.Distance(transform.position, targetPosition);

            yield return null;
        }

        // Snap to target position when close enough
        transform.position = targetPosition;

        // No longer moving
        isMoving = false;

        // Log that we've reached the destination
        Debug.Log("Golfer reached destination");

        // Since in our simple MVP this only happens when reaching a hole, call ReachHole
        ReachHole();
    }

    // Called when the golfer reaches a hole
    public void ReachHole()
    {
        // Increment score
        score++;

        // Log for debugging
        Debug.Log($"Golfer completed hole with score: {score}");

        // Notify GolferSystem before destruction
        if (golferSystem != null)
        {
            golferSystem.RemoveGolfer(this);
        }

        // Destroy the golfer (will be replaced with actual behavior later)
        Destroy(gameObject);
    }

    // Get the current score
    public int GetScore()
    {
        return score;
    }
}
