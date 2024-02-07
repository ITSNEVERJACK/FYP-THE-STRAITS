using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class BuildingPanel : MonoBehaviour
{
    public Transform contentContainer;
    public GameObject buildingTemplatePrefab;

    // Reference to the ItemManager
    public ItemManager itemManager;

    // Callback to notify completion
    private System.Action onBuildingComplete;

    private void Awake()
    {
        // Disable the BuildingTemplate prefab on Awake
        buildingTemplatePrefab.SetActive(false);
    }

    public void StartBuilding(ShipData shipData, System.Action onBuildingCompleteCallback)
    {
        // Deduct build cost from ItemManager.currency immediately
        if (itemManager != null)
        {
            if (itemManager.currency >= shipData.purchaseCost)
            {
                itemManager.currency -= shipData.purchaseCost;
            }
            else
            {
                Debug.LogWarning("Not enough currency to start building the ship.");
                // Optionally, display a warning UI or provide feedback to the player
                return;
            }

            // Update the UI to reflect the changes in currency
            itemManager.UpdateResourceTexts();
        }
        else
        {
            Debug.LogError("ItemManager reference is null.");
            return;
        }

        // Set the callback function
        onBuildingComplete = onBuildingCompleteCallback;

        // Enable the BuildingTemplate prefab
        buildingTemplatePrefab.SetActive(true);

        // Instantiate the BuildingTemplate prefab directly under the contentContainer
        GameObject buildingInstance = Instantiate(buildingTemplatePrefab, contentContainer);
        BuildingTemplate buildingTemplate = buildingInstance.GetComponent<BuildingTemplate>();

        if (buildingTemplate != null)
        {
            buildingTemplate.Initialize(shipData);
        }
        else
        {
            Debug.LogError("BuildingTemplate component not found on instantiated object.");
        }

        DontDestroyOnLoad(gameObject);
        StartCoroutine(BuildShip(shipData, buildingTemplate));
    }

    private IEnumerator BuildShip(ShipData shipData, BuildingTemplate buildingTemplate)
    {
        float timer = 0f;
        float buildTime = shipData.buildTime;

        while (timer < buildTime)
        {
            timer += Time.deltaTime;

            // Update the building template timer
            buildingTemplate.UpdateTimer(buildTime - timer);

            yield return null;
        }

        // Disable the BuildingTemplate prefab after completion
        buildingTemplatePrefab.SetActive(false);

        // Destroy the building template
        Destroy(buildingTemplate.gameObject);
        onBuildingComplete?.Invoke();
    }
}
