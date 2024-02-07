using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    private PlayerInputActions cameraActions;
    private InputAction movement;
    private InputAction rotateCameraAction;
    private InputAction zoomCameraAction;
    private InputAction tabTriggerAction; // Separate action for TabTrigger
    private Transform cameraTransform;

    // Horizontal motion
    [SerializeField]
    private float maxspeed = 5f;
    private float speed;
    [SerializeField]
    private float acceleration = 10f;
    [SerializeField]
    private float damping = 15f;

    // Vertical motion - zooming
    [SerializeField]
    private float stepside = 2f;
    [SerializeField]
    private float zoomDampening = 7.5f;
    [SerializeField]
    private float minHeight = 5f;
    [SerializeField]
    private float maxHeight = 50f;
    [SerializeField]
    private float zoomSpeed = 2f;

    // Rotation
    [SerializeField]
    private float maxRotationSpeed = 1f;

    // Screen edge motion
    [SerializeField]
    [Range(0f, 0.1f)]
    private float edgeTolerance = 0.5f;
    [SerializeField]
    private bool useScreenEdge = true;

    // Value set in various functions
    // Used to update the position of the camera base object
    private Vector3 targetPosition;

    private float zoomHeight;

    // Used to track maintain velocity w/o a rigidbody
    private Vector3 horizontalVelocity;
    private Vector3 lastPosition;

    // Track where the dragging action started 
    private Vector3 startDrag;

    // ... (other variables)

    public GameObject uiObject; // Reference to the UI GameObject
    private bool isUIOpen = false; // Flag to track UI state

    private void Awake()
    {
        cameraActions = new PlayerInputActions();
        cameraTransform = this.GetComponentInChildren<Camera>().transform;
        movement = cameraActions.Player.Movement;
        rotateCameraAction = cameraActions.Player.RotateCamera;
        zoomCameraAction = cameraActions.Player.Zoomcamera;
        tabTriggerAction = cameraActions.Player.TabTrigger;
    }

    private void OnEnable()
    {
        zoomHeight = cameraTransform.localPosition.y;
        cameraTransform.LookAt(this.transform);

        lastPosition = this.transform.position;

        movement.Enable();
        rotateCameraAction.Enable();
        zoomCameraAction.Enable();
        tabTriggerAction.performed += ToggleUI; // Use TabTrigger to toggle UI
        cameraActions.Player.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        rotateCameraAction.Disable();
        zoomCameraAction.Disable();
        tabTriggerAction.performed -= ToggleUI;
        cameraActions.Player.Disable();
    }

    private void Update()
    {
        if (!isUIOpen)
        {
            GetKeyboardMovement();
            CheckMouseAtScreenEdge();
            DragCamera();

            // Rotate and Zoom Camera
            RotateCamera();
            ZoomCamera();

            // Move base and camera objects
            UpdateVelocity();
            UpdateBasePosition();
            UpdateCameraPosition();
        }
    }

    private void RotateCamera()
    {
        if (rotateCameraAction.ReadValue<Vector2>().x != 0f && Mouse.current.middleButton.isPressed)
        {
            float inputValue = rotateCameraAction.ReadValue<Vector2>().x;
            transform.rotation = Quaternion.Euler(0f, inputValue * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
        }
    }

    private void ZoomCamera()
    {
        if (Mathf.Abs(zoomCameraAction.ReadValue<Vector2>().y) > 0.1f)
        {
            float inputValue = -zoomCameraAction.ReadValue<Vector2>().y / 100f;
            ZoomHeight(inputValue);
        }
    }

    private void UpdateVelocity()
    {
        horizontalVelocity = (this.transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0f;
        lastPosition = this.transform.position;
    }

    private void GetKeyboardMovement()
    {
        Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight()
                    + movement.ReadValue<Vector2>().y * GetCameraForward();

        inputValue = inputValue.normalized;

        if (inputValue.sqrMagnitude > 0.1f)
            targetPosition += inputValue;
    }

    private void CheckMouseAtScreenEdge()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if (mousePosition.x < edgeTolerance * Screen.width)
            moveDirection += -GetCameraRight();
        else if (mousePosition.x > (1f - edgeTolerance) * Screen.width)
            moveDirection += GetCameraRight();

        if (mousePosition.y < edgeTolerance * Screen.height)
            moveDirection += -GetCameraForward();
        else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
            moveDirection += GetCameraForward();

        targetPosition += moveDirection;
    }

    private void DragCamera()
    {
        if (!Mouse.current.rightButton.isPressed)
            return;

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = cameraTransform.GetComponent<Camera>().ScreenPointToRay(Mouse.current.position.ReadValue());

        if (plane.Raycast(ray, out float distance))
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
                startDrag = ray.GetPoint(distance);
            else
                targetPosition += startDrag - ray.GetPoint(distance);
        }
    }

    private void UpdateBasePosition()
    {
        if (targetPosition.sqrMagnitude > 0.1f)
        {
            speed = Mathf.Lerp(speed, maxspeed, Time.deltaTime * acceleration);
            transform.position += targetPosition * speed * Time.deltaTime;
        }
        else
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
        }

        targetPosition = Vector3.zero;
    }

    private void ZoomHeight(float inputValue)
    {
        zoomHeight = cameraTransform.localPosition.y + inputValue * stepside;

        if (zoomHeight < minHeight)
            zoomHeight = minHeight;
        else if (zoomHeight > maxHeight)
            zoomHeight = maxHeight;
    }

    private void UpdateCameraPosition()
    {
        Vector3 zoomTarget = new Vector3(cameraTransform.localPosition.x, zoomHeight, cameraTransform.localPosition.z);
        zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.y) * Vector3.forward;

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
        cameraTransform.LookAt(this.transform);
    }

    private void ToggleUI(InputAction.CallbackContext obj)
    {
        if (uiObject != null)
        {
            isUIOpen = !isUIOpen;

            if (isUIOpen)
            {
                // Disable movement when UI is open
                DisableMovement();
                EnableUIObject();
            }
            else
            {
                // Enable movement when UI is closed
                EnableMovement();
                DisableUIObject();
            }
        }
    }

    private void DisableUIObject()
    {
        // Your logic to disable the UI GameObject
        uiObject.SetActive(false);
    }

    private void EnableUIObject()
    {
        // Your logic to enable the UI GameObject
        uiObject.SetActive(true);
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        return forward;
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.y = 0f;
        return right;
    }

    private void DisableMovement()
    {
        // Your logic to disable movement
        speed = 0f;
    }

    private void EnableMovement()
    {
        // Your logic to enable movement
        speed = maxspeed;
    }
}
