using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PriceTracker : MonoBehaviour
{
    public ShopSystem shopSystem;
    public PriceTrackerTemplate itemTemplatePrefab;
    public Transform itemParentTransform; // The parent transform where items will be instantiated
    public Sprite upArrowSprite; // Reference to the up arrow sprite
    public Sprite downArrowSprite; // Reference to the down arrow sprite

    private List<Item> trackedItems = new List<Item>();
    private Dictionary<Item, PriceTrackerTemplate> itemUIElements = new Dictionary<Item, PriceTrackerTemplate>();

    private void Start()
    {
        // Add the items you want to track to the list.
        trackedItems.AddRange(shopSystem.shopItems);

        // Instantiate UI elements for each tracked item based on the template.
        foreach (Item itemData in trackedItems)
        {
            PriceTrackerTemplate itemGO = Instantiate(itemTemplatePrefab, itemParentTransform);

            // Customize the item UI with item-specific data (e.g., icon and price).
            itemGO.SetItemData(itemData);

            // Set the arrow sprites in the template.
            itemGO.SetArrowSprite(upArrowSprite, downArrowSprite);

            itemUIElements[itemData] = itemGO;
        }

        // Disable the original template GameObject.
        itemTemplatePrefab.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Update prices for all tracked items in real-time.
        foreach (Item itemData in trackedItems)
        {
            UpdateItemPrice(itemData);
        }
    }

    private void UpdateItemPrice(Item itemData)
    {
        // Retrieve the UI element for the item.
        if (itemUIElements.TryGetValue(itemData, out PriceTrackerTemplate itemGO))
        {
            int currentPrice = itemData.currentPrice; // Get the current price from the Item script
            int previousBasePrice = itemData.basePrice; // Get the base price from the Item script

            // Compare current price with the previous base price.
            if (currentPrice > previousBasePrice)
            {
                // Price increased, set up arrow flag to true and down arrow flag to false
                itemGO.showUpArrow = true;
                itemGO.showDownArrow = false;
            }
            else if (currentPrice < previousBasePrice)
            {
                // Price decreased, set up arrow flag to false and down arrow flag to true
                itemGO.showUpArrow = false;
                itemGO.showDownArrow = true;
            }
            else
            {
                // Price unchanged, set both arrow flags to false
                itemGO.showUpArrow = false;
                itemGO.showDownArrow = false;
            }

            // Update arrow visibility based on the flags
            itemGO.UpdateArrowVisibility();

            // Update the displayed price using TextMesh Pro.
            itemGO.SetPriceText("Price: " + currentPrice);
        }
    }
}
