using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LocationDisplay : MonoBehaviour
{
    public GameObject locationTemplatePrefab; // Assign the template GameObject in the Inspector
    private TradingSystem tradingSystem;
    private LocationDisplayManager locationDisplayManager;

    private void Start()
    {
        tradingSystem = FindObjectOfType<TradingSystem>();
        locationDisplayManager = FindObjectOfType<LocationDisplayManager>();

        if (tradingSystem == null)
        {
            Debug.LogError("TradingSystem not found in the scene.");
            return;
        }

        locationTemplatePrefab.SetActive(false); // Disable the prefab at the start
        GenerateLocationInstances();
    }

    private void GenerateLocationInstances()
    {
        locationTemplatePrefab.SetActive(true); // Re-enable the prefab before generating location instances

        foreach (TradingSystem.Location location in tradingSystem.locations)
        {
            GameObject locationInstance = Instantiate(locationTemplatePrefab, transform);
            locationInstance.SetActive(true);

            // Get the LocationTemplate component attached to the instantiated GameObject
            LocationTemplate locationTemplateComponent = locationInstance.GetComponent<LocationTemplate>();

            if (locationTemplateComponent != null)
            {
                // Set the location name using the SetLocationInfo method
                locationTemplateComponent.SetLocationInfo(location.locationName);
            }

            // Add a button click event or other input method to call the correct method
            Button button = locationInstance.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => DisplayLocation(location));
            }
        }

        locationTemplatePrefab.SetActive(false); // Disable the prefab after generating location instances
    }

    private void DisplayLocation(TradingSystem.Location location)
    {
        locationDisplayManager.GenerateItemDisplayTemplates(location);
    }
}
