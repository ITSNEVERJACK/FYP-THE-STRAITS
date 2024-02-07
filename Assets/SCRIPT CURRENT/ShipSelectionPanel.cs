using UnityEngine;
using System.Collections.Generic;

public class ShipSelectionPanel : MonoBehaviour
{
    public ShipCommisionPanel shipCommisionPanel;
    public GameObject shipSelectTemplatePrefab;
    public List<BuildingPanel> buildingPanels;
   

    public Transform contentContainer;

    private ItemManager itemManager;
    public ShipData[] availableShips;

    private void Start()
    {
        itemManager = FindObjectOfType<ItemManager>();
        if (itemManager == null)
        {
            Debug.LogError("ItemManager not found in the scene.");
        }

        GenerateShipButtons();
       
    }

    private void GenerateShipButtons()
    {
        foreach (ShipData shipData in availableShips)
        {
            shipSelectTemplatePrefab.SetActive(true);

            ShipSelectTemplate shipButton = Instantiate(shipSelectTemplatePrefab, contentContainer).GetComponent<ShipSelectTemplate>();
            if (shipButton != null)
            {
                shipButton.Initialize(shipData, this);
            }
            else
            {
                Debug.LogError("ShipSelectTemplate component not found on instantiated object.");
            }

            shipSelectTemplatePrefab.SetActive(false);
        }
    }

    public void OnShipSelected(ShipData shipData)
    {
        shipCommisionPanel.UpdateShipInfo(shipData);
    }

   
}
