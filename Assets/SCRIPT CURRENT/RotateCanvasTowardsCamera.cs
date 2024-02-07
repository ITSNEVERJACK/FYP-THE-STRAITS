using UnityEngine;

public class RotateCanvasTowardsCamera : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the camera object

    void Update()
    {
        // Calculate the direction from the canvas to the camera
        Vector3 directionToCamera = cameraTransform.position - transform.position;

        // Rotate the canvas to face the camera
        transform.rotation = Quaternion.LookRotation(-directionToCamera, Vector3.up);
    }
}
