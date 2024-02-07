using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopSystem : MonoBehaviour
{
    public GameObject shopTemplatePrefab;
    public ItemManager itemManager;
    public Item[] shopItems;
    private Dictionary<Item, int> availableQuantities = new Dictionary<Item, int>();
    private Dictionary<Item, float> timeSinceLastRestock = new Dictionary<Item, float>();
    private Dictionary<Item, int> purchaseAmounts = new Dictionary<Item, int>();
    private Dictionary<Item, float> timeSinceLastPurchase = new Dictionary<Item, float>();
    private Dictionary<Item, float> timeSinceLastPriceReset = new Dictionary<Item, float>();

    
    public float discountMultiplier = 0.9f;
    private bool isDiscountEnabled = false;
    public float itemRestockTime = 300.0f;
    public float priceFluctuationRate = 0.01f;
    public float maxPriceMultiplier = 2.0f;

    [SerializeField] private Transform shopItemsParent;  // Serialized field for the parent transform

    private void Start()
    {
        GenerateShopItems();

        foreach (Item itemData in shopItems)
        {
            availableQuantities[itemData] = itemData.initialQuantity;
            timeSinceLastRestock[itemData] = 0.0f;
            purchaseAmounts[itemData] = 0;
            timeSinceLastPurchase[itemData] = 0.0f;
            timeSinceLastPriceReset[itemData] = 0.0f;
        }
    }

    private void GenerateShopItems()
    {
        if (itemManager == null || shopTemplatePrefab == null || shopItemsParent == null)
        {
            Debug.LogError("ItemManager reference, ShopTemplate prefab, or ShopItemsParent transform is not set in the Inspector!");
            return;
        }

        foreach (Item itemData in shopItems)
        {
            itemData.currentPrice = itemData.basePrice;
            GameObject shopTemplateGO = Instantiate(shopTemplatePrefab, shopItemsParent);  // Use the serialized field as the parent transform
            ShopTemplate shopTemplate = shopTemplateGO.GetComponent<ShopTemplate>();

            shopTemplate.SetAvailableQuantity(itemData.initialQuantity);

            shopTemplate.NameText.text = itemData.itemName;
            shopTemplate.DescriptionText.text = itemData.description;
            shopTemplate.Pricetext.text = "Price: " + CalculateItemPrice(itemData);

            shopTemplate.ItemImage.sprite = itemData.icon;

            shopTemplate.UpdateQuantityText(itemData);

            shopTemplate.ItemManagerReference = itemManager;

            int availableQty;
            if (availableQuantities.TryGetValue(itemData, out availableQty))
            {
                shopTemplate.SetAvailableQuantity(availableQty);
            }
            else
            {
                // Handle the case when available quantity is not found in the dictionary
            }

            Button buyButton = shopTemplateGO.GetComponent<Button>();
            buyButton.onClick.AddListener(() => ShowPurchaseUI(itemData, shopTemplate));
        }

        shopTemplatePrefab.SetActive(false);
    }

    public void ShowPurchaseUI(Item itemData, ShopTemplate shopTemplate)
    {
        BuyItemUI.Instance.Open(itemData, this, shopTemplate, GetAvailableQuantity(itemData));
        int price = CalculateItemPrice(itemData);
        int availability = GetAvailableQuantity(itemData);
        shopTemplate.UpdatePriceAndAvailabilityText(price, availability);
    }

    public void BuyItem(Item itemData, ShopTemplate shopTemplate, int requestedQuantity)
    {
        int availableQty = GetAvailableQuantity(itemData);

        if (requestedQuantity > 0 && requestedQuantity <= availableQty)
        {
            int itemPrice = CalculateItemPrice(itemData);

            if (itemPrice <= 0)
            {
                Debug.LogWarning("Item price is zero or negative.");
                return;
            }

            int playerCurrency = itemManager.currency;

            int totalCost = itemPrice * requestedQuantity;

            if (playerCurrency >= totalCost)
            {
                itemManager.AddItem(itemData, requestedQuantity); // Updated line
                itemManager.currency -= totalCost;
                int quantityOwned = itemManager.GetItemQuantity(itemData); // Updated line
                shopTemplate.UpdateQuantityText(itemData);

                availableQty -= requestedQuantity;
                availableQuantities[itemData] = availableQty;

                purchaseAmounts[itemData] += requestedQuantity;
                timeSinceLastPurchase[itemData] = 0.0f;

                UpdateItemPriceBasedOnPurchaseAmount(itemData);

                int price = CalculateItemPrice(itemData);
                shopTemplate.UpdatePriceAndAvailabilityText(price, availableQty);

                UpdateUIToReflectAvailableQuantity(itemData, availableQty);
                UpdateUIToReflectPrice(itemData, price);
            }
            else
            {
                Debug.LogWarning("Not enough currency to buy " + itemData.itemName);
            }
        }
        else
        {
            Debug.LogWarning("Invalid or unavailable quantity requested.");
        }
    }


    public int GetAvailableQuantity(Item itemData)
    {
        if (availableQuantities.TryGetValue(itemData, out int availableQuantity))
        {
            return availableQuantity;
        }
        return 0;
    }

    public void SubtractAvailableQuantity(Item itemData, int quantity)
    {
        if (availableQuantities.ContainsKey(itemData))
        {
            availableQuantities[itemData] = Mathf.Max(0, availableQuantities[itemData] - quantity);
        }
        else
        {
            Debug.LogWarning("ItemData not found in availableQuantities dictionary.");
        }
    }

    private int CalculateItemPrice(Item itemData)
    {
        int basePrice = itemData.currentPrice;

        if (isDiscountEnabled)
        {
            basePrice = Mathf.RoundToInt(basePrice * discountMultiplier);
        }

        return basePrice;
    }

    public void SetDiscountEnabled(bool isEnabled)
    {
        isDiscountEnabled = isEnabled;
    }
    private void RestockItem(Item itemData)
    {
        availableQuantities[itemData] = itemData.initialQuantity;
        timeSinceLastRestock[itemData] = 0.0f;
        UpdateUIToReflectAvailableQuantity(itemData, itemData.initialQuantity);
    }

    private void UpdateUIToReflectAvailableQuantity(Item itemData, int availableQuantity)
    {
        foreach (Transform child in shopItemsParent)
        {
            ShopTemplate shopTemplate = child.GetComponent<ShopTemplate>();
            if (shopTemplate != null && shopTemplate.GetItem() == itemData)
            {
                shopTemplate.SetAvailableQuantity(availableQuantity);
                break;
            }
        }
    }

    private void UpdateUIToReflectPrice(Item itemData, int price)
    {
        foreach (Transform child in shopItemsParent)
        {
            ShopTemplate shopTemplate = child.GetComponent<ShopTemplate>();
            if (shopTemplate != null && shopTemplate.GetItem() == itemData)
            {
                shopTemplate.Pricetext.text = "Price: " + price;
                break;
            }
        }
    }

    private void UpdateItemPriceBasedOnPurchaseAmount(Item itemData)
    {
        int basePrice = itemData.currentPrice;
        int purchaseAmount = purchaseAmounts[itemData];
        float priceMultiplier = Mathf.Clamp(1.0f + (purchaseAmount * priceFluctuationRate), 1.0f, maxPriceMultiplier);
        int newPrice = Mathf.RoundToInt(basePrice * priceMultiplier);
        itemData.currentPrice = newPrice;
    }

}
