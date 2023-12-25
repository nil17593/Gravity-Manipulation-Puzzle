using UnityEngine;

namespace Parodystudios.GravityModificationPuzzle
{
    /// <summary>
    /// CameraFollow:
    /// - Controls the camera to follow and look at a specified target object.
    /// - Allows the camera to orbit around the target based on mouse movement.
    /// - Defines boundaries for vertical rotation to limit the camera's angle.
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        private const float YMin = -50.0f;
        private const float YMax = 50.0f;
        private float currentX = 0.0f;
        private float currentY = 0.0f;

        [SerializeField] private Transform lookAt;
        [SerializeField] private float distance = 10.0f;
        [SerializeField] private float sensitivity = 100.0f;

        void LateUpdate()
        {
            // Update the current X and Y rotation angles based on mouse input
            currentX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            currentY += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            // Clamp the vertical rotation angle within defined limits
            currentY = Mathf.Clamp(currentY, YMin, YMax);

            // Calculate the camera's position based on distance, rotation, and target position
            Vector3 direction = new Vector3(0, 0, distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            transform.position = lookAt.position + rotation * direction;

            // Make the camera look at the target position
            transform.LookAt(lookAt.position);
        }
    }
}