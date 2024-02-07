using UnityEngine;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour
{
    // Reference to the BuildingPanel prefab
    public GameObject buildingPanelPrefab;

    // Instance of the BuildingPanel
    private GameObject buildingPanelInstance;

    // Reference to the ItemManager
    public ItemManager itemManager;

    private void Start()
    {
       
        itemManager = FindObjectOfType<ItemManager>();
        if (itemManager == null)
        {
            Debug.LogError("ItemManager not found in the scene.");
        }
    }

    public void StartBuilding(ShipData shipData)
    {
        // Deduct build cost from ItemManager.currency
        if (itemManager != null)
        {
            if (itemManager.currency >= shipData.purchaseCost)
            {
                itemManager.currency -= shipData.purchaseCost;
            }
            else
            {
                Debug.LogError("Not enough currency to start building the ship.");
                return;
            }
        }
        else
        {
            Debug.LogError("ItemManager reference is null.");
            return;
        }

        // Instantiate the BuildingPanel if it's not instantiated yet
        if (buildingPanelInstance == null)
        {
            buildingPanelInstance = Instantiate(buildingPanelPrefab);
        }

        // Call the StartBuilding method on the BuildingPanel
        BuildingPanel buildingPanel = buildingPanelInstance.GetComponent<BuildingPanel>();
        if (buildingPanel != null)
        {
            // Pass the required parameters to StartBuilding
            buildingPanel.StartBuilding(shipData, () => OnShipBuilt(shipData));
        }
        else
        {
            Debug.LogError("BuildingPanel component not found on instantiated object.");
        }
    }

    private void OnShipBuilt(ShipData shipData)
    {
        // Notify completion of ship building
        Debug.Log("Ship built: " + shipData.GivenName);

        // Add the ship to the item manager or handle as needed
        if (itemManager != null)
        {
            // Use the correct method name: AddShip
            itemManager.AddShip(shipData);
        }
        else
        {
            Debug.LogError("ItemManager reference is null.");
        }
    }
}
