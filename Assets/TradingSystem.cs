using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TradingSystem : MonoBehaviour
{
    [System.Serializable]
    public class Location
    {
        public string locationName;
        public Dictionary<Item, int> availableQuantities = new Dictionary<Item, int>();
        public List<Item> sellingItems = new List<Item>();
        public List<Item> buyingItems = new List<Item>();
        
        public float discountMultiplier = 0.9f;
        public bool isDiscountEnabled = false;
        public float itemRestockTime = 300.0f;
        public float priceFluctuationRate = 0.01f;
        public float maxPriceMultiplier = 2.0f;
        public int maxItemsCap = 4;
    }

    public List<Location> locations = new List<Location>();
    public List<Item> tradingItems = new List<Item>();

    private void Start()
    {
        InitializeLocations();
        StartCoroutine(UpdateTradingItemsRoutine());
    }

    private void InitializeLocations()
    {
        foreach (Location location in locations)
        {
            location.sellingItems = GetRandomItems();
            location.buyingItems = new List<Item>(location.sellingItems.Take(location.maxItemsCap));
            StartCoroutine(RestockItemsRoutine(location));
            
            Debug.Log($"Initialized location: {location.locationName}");
        }
    }


    private IEnumerator RestockItemsRoutine(Location location)
    {
        while (true)
        {
            yield return new WaitForSeconds(location.itemRestockTime);

            foreach (Item item in location.sellingItems)
            {
                location.availableQuantities[item] = Random.Range(1, 10);
            }

            Debug.Log($"Items restocked at {location.locationName}");
        }
    }

    

    private IEnumerator UpdateTradingItemsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1800);

            // Rotate the items for each location
            foreach (Location location in locations)
            {
                RotateItems(location.sellingItems);
                location.buyingItems = new List<Item>(location.sellingItems.Take(location.maxItemsCap));
            }
        }
    }

    private void RotateItems(List<Item> items)
    {
        // Move the first item to the end of the list
        Item rotatedItem = items.First();
        items.RemoveAt(0);
        items.Add(rotatedItem);
    }

    private List<Item> GetRandomItems()
    {
        List<Item> allItems = tradingItems.ToList();
        ShuffleList(allItems);

        for (int i = 0; i < locations.Count; i++)
        {
            // Calculate the number of items needed for each list
            int itemsForSelling = Mathf.Min(locations[i].maxItemsCap, allItems.Count);
            int itemsForBuying = Mathf.Min(locations[i].maxItemsCap, allItems.Count - itemsForSelling);

            // Get a random subset of items for selling
            locations[i].sellingItems = allItems.Take(itemsForSelling).ToList();

            // Update the remaining items for buying
            locations[i].buyingItems = allItems.Skip(itemsForSelling).Take(itemsForBuying).ToList();

            // Remove the used items from the pool
            allItems = allItems.Skip(itemsForSelling + itemsForBuying).ToList();
        }

        // Return the selling items for the first location (you can modify this based on your requirements)
        return locations.FirstOrDefault()?.sellingItems;
    }



    private void ShuffleList<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
