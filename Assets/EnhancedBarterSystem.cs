using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class EnhancedBarterSystem : MonoBehaviour
{
    public Image tradeItemImage;
    public TextMeshProUGUI quantityText;

    public List<TraderOfferSlot> traderOfferSlots = new List<TraderOfferSlot>(); // List of trader offer slots

    public ItemManager itemManager; // Reference to the item manager to access trader's currency
    public Item[] tradeItems;
    private Item currentTradeItem; // Current trade item selected

    private Dictionary<Item, int> availableQuantities = new Dictionary<Item, int>();
    private Dictionary<Item, float> timeSinceLastTrade = new Dictionary<Item, float>();
    private Dictionary<Item, int> tradeAmounts = new Dictionary<Item, int>();
    private Dictionary<Item, float> timeSinceLastPriceReset = new Dictionary<Item, float>();

    public float exchangeMultiplier = 1.5f;
    public float tradeInterval = 300.0f; // 5 minutes
    public float priceFluctuationRate = 0.01f;
    public float maxPriceMultiplier = 2.0f;

    public float reputationMultiplier = 1.0f; // Reputation starts at 1.0

    private void Start()
    {
        InitializeTradeItems();
        InitializeTraderOfferSlots();
        SwitchTradeItem(traderOfferSlots[0].tradeItem); // Set the initial trade item
        UpdateUIToReflectCurrentTradeItem();
    }

    private void InitializeTradeItems()
    {
        // Initialize dictionaries and other data structures as needed
    }

    private void InitializeTraderOfferSlots()
    {
        // Randomize trader offer slots with items and quantities
        foreach (TraderOfferSlot slot in traderOfferSlots)
        {
            slot.tradeItem = GetRandomTradeItem();
            slot.offeredQuantity = GetRandomQuantity();
        }
    }

    private Item GetRandomTradeItem()
    {
        // Randomize and return a trade item from available trade items
        return tradeItems[UnityEngine.Random.Range(0, tradeItems.Length)];
    }

    private int GetRandomQuantity()
    {
        // Randomize and return a quantity value
        return UnityEngine.Random.Range(1, 10); // Example: Randomize between 1 and 10
    }

    public void SwitchTradeItem(Item newTradeItem)
    {
        currentTradeItem = newTradeItem;
        UpdateUIToReflectCurrentTradeItem();
    }

    public Item GetCurrentTradeItem()
    {
        return currentTradeItem;
    }

    public void MakeTrade(List<PlayerTradeOffer.PlayerOfferSlot> playerOfferSlots, List<TraderOfferSlot> traderOfferSlots, float playerReputation)
    {
        if (currentTradeItem != null)
        {
            int totalPlayerOfferedQty = 0;
            int totalTraderOfferedQty = 0;

            // Calculate the total quantity offered by the player
            foreach (PlayerTradeOffer.PlayerOfferSlot offerSlot in playerOfferSlots)
            {
                totalPlayerOfferedQty += offerSlot.quantity;
            }

            // Calculate the total quantity offered by the trader
            foreach (TraderOfferSlot offerSlot in traderOfferSlots)
            {
                totalTraderOfferedQty += offerSlot.offeredQuantity;
            }

            // Get the TradeItemSlot for the current trade item
            TradeItemSlot selectedSlot = tradeItemSlots.Find(slot => slot.tradeItem == currentTradeItem);

            if (selectedSlot != null)
            {
                // Ensure there is enough quantity available for trade from both player and trader
                if (availableQuantities[currentTradeItem] >= totalPlayerOfferedQty && totalTraderOfferedQty > 0)
                {
                    // Calculate the trade value
                    int itemValue = CalculateTradeValue(currentTradeItem, playerReputation);
                    int totalPlayerValue = itemValue * totalPlayerOfferedQty;

                    // Get the trader's currency
                    int traderValue = itemManager.currency;

                    // Check if the trader has enough currency to make the trade
                    if (traderValue >= totalPlayerValue)
                    {
                        // Perform the trade
                        availableQuantities[currentTradeItem] -= totalPlayerOfferedQty;
                        tradeAmounts[currentTradeItem] += totalPlayerOfferedQty;
                        timeSinceLastTrade[currentTradeItem] = 0.0f;

                        // Update the trader's inventory or other relevant actions based on traderOfferSlots
                        foreach (TraderOfferSlot offerSlot in traderOfferSlots)
                        {
                            // Update the trader's inventory or perform relevant actions
                        }

                        // Update the UI
                        UpdateUIToReflectCurrentTradeItem();
                    }
                    else
                    {
                        Debug.LogWarning("Not enough currency for the trader to make the trade for " + currentTradeItem.itemName);
                    }
                }
                else
                {
                    Debug.LogWarning("Not enough quantity available for trade from the player or trader for " + currentTradeItem.itemName);
                }
            }
            else
            {
                Debug.LogWarning("Selected trade item not found in the trade item slots.");
            }
        }
        else
        {
            Debug.LogWarning("No trade item selected.");
        }
    }

    private int CalculateTradeValue(Item tradeItem, float playerReputation)
    {
        // Modify the value calculation based on player reputation
        int baseValue = tradeItem.currentPrice;

        // Adjust value based on player reputation
        float reputationAdjustedMultiplier = Mathf.Clamp(playerReputation, 0.5f, 2.0f);

        int adjustedValue = Mathf.RoundToInt(baseValue * reputationAdjustedMultiplier);

        return adjustedValue;
    }

    private void UpdateUIToReflectCurrentTradeItem()
    {
        if (currentTradeItem != null)
        {
            tradeItemImage.sprite = currentTradeItem.icon;
            quantityText.text = "Quantity: " + availableQuantities[currentTradeItem];
        }
    }

    [Serializable]
    public class TraderOfferSlot
    {
        public Item tradeItem;
        public int offeredQuantity;
    }
}
