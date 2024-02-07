using UnityEngine;
using System.Collections.Generic;

public class InvShipGenerator : MonoBehaviour
{
    public GameObject shipTemplatePrefab;
    public Transform shipListParent;

    private List<InvShipInstance> generatedShips = new List<InvShipInstance>();

    [System.Serializable]
    public class InvShipInstance
    {
        public GameObject instance;
        public ShipData shipData;
        public InvShipTemplate shipTemplate;
    }

    private void Start()
    {
        GenerateInvShipInstances();
    }

    private void GenerateInvShipInstances()
    {
        // Clear existing generated ships
        foreach (InvShipInstance shipInstance in generatedShips)
        {
            Destroy(shipInstance.instance);
        }

        generatedShips.Clear();

        // Get the updated list of ships
        List<ShipData> ships = GetShips();

        foreach (ShipData ship in ships)
        {
            GameObject shipInstance = Instantiate(shipTemplatePrefab, shipListParent);
            InvShipTemplate shipTemplate = shipInstance.GetComponent<InvShipTemplate>();

            if (shipTemplate != null)
            {
                shipTemplate.Initialize(ship, this);

                InvShipInstance instanceData = new InvShipInstance();
                instanceData.instance = shipInstance;
                instanceData.shipData = ship;
                instanceData.shipTemplate = shipTemplate;

                generatedShips.Add(instanceData);
            }
            else
            {
                Debug.LogWarning("InvShipTemplate component not found on ship template prefab.");
            }
        }
    }

    private List<ShipData> GetShips()
    {
        ItemManager itemManager = FindObjectOfType<ItemManager>();
        if (itemManager != null)
        {
            // Return your list of ships here (modify based on your game logic)
            return itemManager.shipsList;
        }
        else
        {
            Debug.LogError("ItemManager not found in the scene.");
            return new List<ShipData>();
        }
    }
    public void OnShipSelected(ShipData selectedShip)
    {
        
        Debug.Log("Selected Ship: " + selectedShip.GivenName);
        
    }
}
