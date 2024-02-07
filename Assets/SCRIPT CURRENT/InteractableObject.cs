using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    private bool canvasActive;

    private void Awake()
    {
        // Initialize the canvas state
        canvas.gameObject.SetActive(false);
        canvasActive = false;

        // Add a BoxCollider component if one doesn't exist
        if (!GetComponent<BoxCollider>())
            gameObject.AddComponent<BoxCollider>();
    }

    private void OnMouseDown()
    {
        if (canvasActive)
        {
            // Canvas is already active, so close it
            canvas.gameObject.SetActive(false);
            canvasActive = false;
        }
        else
        {
            // Canvas is not active, so open it
            canvas.gameObject.SetActive(true);
            canvasActive = true;
        }
    }
}
