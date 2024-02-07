using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RouteAdder : MonoBehaviour
{
    public TMP_InputField routeNameInput;
    public TMP_Dropdown destinationDropdown;
    
    public Button saveButton;
    public Button cancelButton;

    private RouteEditor shipRouteEditor;

    public void SetShipRouteEditor(RouteEditor editor)
    {
        shipRouteEditor = editor;
    }

    

    public void OnSaveButtonClicked()
    {
        // Create a new route based on the input fields and dropdown
        Route newRoute = new Route
        {
            routeName = routeNameInput.text,
            // Set other route properties based on the UI inputs
        };

        // Add the route to the selected ship
        shipRouteEditor.SetSelectedShipRoute(newRoute);

        // Close or reset the UI
        Destroy(gameObject);
    }

    public void OnCancelButtonClicked()
    {
        // Close or reset the UI without saving the route
        Destroy(gameObject);
    }
}
