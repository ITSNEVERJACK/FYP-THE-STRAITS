using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShipCommisionPanel : MonoBehaviour
{
    [SerializeField]
    private ShipSelectionPanel shipSelectionPanel;

    [SerializeField]
    public BuildingManager buildingManager;

    public TextMeshProUGUI shipNameText;
    public TextMeshProUGUI shipDescriptionText;
    public TextMeshProUGUI shipBuildTimeText;
    public TextMeshProUGUI shipBuildCostText;

    private ShipData currentShipData;

    [SerializeField]
    private Button buildButton; // Reference to the build button

    private void Awake()
    {
        // Ensure that the panel is initially inactive
        gameObject.SetActive(false);
    }

    public void UpdateShipInfo(ShipData shipData)
    {
        currentShipData = shipData;

        shipNameText.text = "Ship: " + shipData.GivenName;
        shipDescriptionText.text = "Description: " + shipData.description;
        shipBuildTimeText.text = "Build Time: " + shipData.buildTime.ToString() + " seconds";
        shipBuildCostText.text = "Build Cost: " + shipData.purchaseCost.ToString() + " currency";

        // Set the panel active when ship data is updated
        gameObject.SetActive(true);
    }

    public void StartBuilding()
    {
        if (buildingManager != null)
        {
            buildingManager.StartBuilding(currentShipData);
        }
        else
        {
            Debug.LogWarning("BuildingManager reference is null.");
        }
    }

    public void SetShipSelectionPanel(ShipSelectionPanel selectionPanel)
    {
        shipSelectionPanel = selectionPanel;
    }

    public void SetBuildButton(Button button)
    {
        buildButton = button;
    }

    public void OnBuildButtonClick()
    {
        StartBuilding();
    }
}
