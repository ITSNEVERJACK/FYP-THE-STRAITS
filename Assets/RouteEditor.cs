using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RouteEditor : MonoBehaviour
{
    public TMP_Text shipNameText;
    public Image shipImage;
    public Button addRouteButton;
    public RouteAdder routeAdderPrefab;
    public Transform routeAdderParent;

    private ShipData selectedShip;

    private void Awake()
    {
        
         RouteEditor Instance = this;
    }

    public void SetSelectedShip(ShipData ship)
    {
        selectedShip = ship;
        shipNameText.text = selectedShip.GivenName;
        shipImage.sprite = selectedShip.shipImage;
        addRouteButton.interactable = true; 
    }

    public void OnAddRouteButtonClicked()
    {
        
        RouteAdder routeAdder = Instantiate(routeAdderPrefab, routeAdderParent);

        
        routeAdder.SetShipRouteEditor(this);
    }

    public void SetSelectedShipRoute(Route newRoute)
    {
        // Add the new route to the selected ship's list
        selectedShip.routes.Add(newRoute);

        
    }
}
