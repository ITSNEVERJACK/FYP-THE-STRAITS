using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceManager : MonoBehaviour
{
    [SerializeField] private float priceResetDuration = 600.0f;
    [SerializeField] private float demandMultiplier = 1.5f;
    [SerializeField] private float demandSwitchDuration = 1200.0f;

    private List<Item> allItems = new List<Item>();
    private Dictionary<Item, float> timeSinceLastPriceReset = new Dictionary<Item, float>();
    public List<Item> itemsInDemand = new List<Item>();

    private void Start()
    {
        ShopSystem shopSystem = FindObjectOfType<ShopSystem>();
        TradingSystem tradingSystem = FindObjectOfType<TradingSystem>();

        if (shopSystem == null || tradingSystem == null)
        {
            Debug.LogError("ShopSystem or TradingSystem not found in the scene. Make sure they are present and active.");
            return;
        }

        allItems.AddRange(shopSystem.shopItems);
        allItems.AddRange(tradingSystem.tradingItems);

        foreach (Item item in allItems)
        {
            timeSinceLastPriceReset[item] = 0.0f;

            if (Random.Range(0f, 1f) < 0.2f)
            {
                itemsInDemand.Add(item);
            }
        }

        StartCoroutine(ResetPricesRoutine());
        StartCoroutine(SwitchInDemandItemsRoutine());
    }

    private IEnumerator ResetPricesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(priceResetDuration);

            foreach (Item item in allItems)
            {
                if (item.currentPrice != item.basePrice)
                {
                    ResetItemPrice(item);
                }
            }

            foreach (Item item in itemsInDemand)
            {
                if (item.currentPrice != item.basePrice)
                {
                    ResetItemPrice(item, demandMultiplier);
                }
            }
        }
    }

    private IEnumerator SwitchInDemandItemsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(demandSwitchDuration);

            itemsInDemand.Clear();

            foreach (Item item in allItems)
            {
                if (Random.Range(0f, 1f) < 0.2f)
                {
                    itemsInDemand.Add(item);
                }
            }
        }
    }

    private void ResetItemPrice(Item item, float multiplier = 1.0f)
    {
        item.currentPrice = Mathf.RoundToInt(item.basePrice * multiplier);
        Debug.Log($"Price reset for {item.itemName}. New price: {item.currentPrice}");
    }
}
