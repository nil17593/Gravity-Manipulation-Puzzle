using UnityEngine;

public class GravityController : MonoBehaviour
{
    public Vector3 currentGravityDirection = Vector3.down; // Default gravity direction
    public Transform world; // Reference to the entire world

    void Update()
    {
        // Detect input to change gravity direction (replace this with your logic)
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Change gravity direction arbitrarily when 'G' key is pressed
            ChangeGravityDirection();
        }
    }

    void ChangeGravityDirection()
    {
        // Simulate gravity direction change by rotating the world
        currentGravityDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        world.rotation = Quaternion.FromToRotation(Vector3.down, currentGravityDirection);
    }
}
