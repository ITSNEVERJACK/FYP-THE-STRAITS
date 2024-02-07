using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LocationDisplayManager : MonoBehaviour
{
    public GameObject itemDisplayTemplatePrefab;
    public Transform itemSoldParentTransform;
    public Transform itemBuyingParentTransform;
    public TextMeshProUGUI locationNameText;

    private void Start()
    {
        TradingSystem tradingSystem = FindObjectOfType<TradingSystem>();

        if (tradingSystem != null && tradingSystem.locations.Count > 0)
        {
            itemDisplayTemplatePrefab.SetActive(false); // Disable the prefab at the start
            GenerateItemDisplayTemplates(tradingSystem.locations[0]);
        }
        else
        {
            Debug.LogError("TradingSystem or its locations not found in the scene.");
        }
    }

    public void GenerateItemDisplayTemplates(TradingSystem.Location location)
    {
        ClearItems(itemSoldParentTransform);
        ClearItems(itemBuyingParentTransform);

        locationNameText.text = location.locationName;

        itemDisplayTemplatePrefab.SetActive(true); // Re-enable the prefab before generating item display templates

        // Display sold items
        foreach (Item item in location.sellingItems)
        {
            GameObject itemDisplayInstance = Instantiate(itemDisplayTemplatePrefab, itemSoldParentTransform);
            ItemDisplayTemplate itemDisplayTemplate = itemDisplayInstance.GetComponent<ItemDisplayTemplate>();

            if (itemDisplayTemplate != null)
            {
                int quantity = location.availableQuantities.ContainsKey(item) ? location.availableQuantities[item] : 0;
                itemDisplayTemplate.SetItemInfo(item, quantity, item.currentPrice);
                itemDisplayTemplate.SetItemImage(item.icon); // Set the item image
            }
        }

        // Display buying items
        foreach (Item item in location.buyingItems)
        {
            GameObject itemDisplayInstance = Instantiate(itemDisplayTemplatePrefab, itemBuyingParentTransform);
            ItemDisplayTemplate itemDisplayTemplate = itemDisplayInstance.GetComponent<ItemDisplayTemplate>();

            if (itemDisplayTemplate != null)
            {
                itemDisplayTemplate.SetItemInfo(item, 0, item.currentPrice); // Set quantity to 0 for buying items
                itemDisplayTemplate.SetItemImage(item.icon); // Set the item image
            }
        }

        itemDisplayTemplatePrefab.SetActive(false); // Disable the prefab after generating item display templates
    }

    private void ClearItems(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
