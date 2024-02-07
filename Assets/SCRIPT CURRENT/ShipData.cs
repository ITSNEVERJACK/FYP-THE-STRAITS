using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "New Ship", menuName = "Ship/ShipData")]
public class ShipData : ScriptableObject
{
    public enum ShipClassType
    {
        Raft, Sloop, Junk, Galleon
    }

    public ShipClassType classType;
    public int itemSlots;
    public GameObject modelPrefab;
    public string description;
    public float speed;
    public int buildTime;
    public int purchaseCost;
    public string GivenName;

    public Sprite shipImage;

    // New inventory system for the ship
    public ShipInventory shipInventory;

    // Routes for the ship
    [SerializeField]
    public List<Route> routes = new List<Route>();
}

[System.Serializable]
public class Route
{
    public string routeName;
    public List<string> routeDestinationNames = new List<string>(); // List of location names
    public List<Item> sellingItems = new List<Item>();
    public List<Item> buyingItems = new List<Item>();
}

[System.Serializable]
public class ShipInventory
{
    public List<ShipInventorySlot> itemSlots;
    public int maxItemSlots; // Maximum number of ship inventory slots

    public ShipInventory(int maxItemSlots)
    {
        this.maxItemSlots = maxItemSlots;
        itemSlots = new List<ShipInventorySlot>();
    }

    public void AddItemToShip(Item item, int quantity)
    {
        if (itemSlots.Count < maxItemSlots)
        {
            ShipInventorySlot existingSlot = itemSlots.Find(slot => slot.item == item);

            if (existingSlot != null)
            {
                existingSlot.quantity += quantity;
            }
            else
            {
                itemSlots.Add(new ShipInventorySlot(item, quantity));
            }
        }
        else
        {
            Debug.LogWarning("Reached the cap for ship item slots");
        }
    }

    public bool HasEnoughSpaceForItem(Item item, int quantity)
    {
        int availableSpace = maxItemSlots - itemSlots.Sum(slot => slot.quantity);
        return availableSpace >= quantity;
    }

    public bool HasItem(Item item, int quantity)
    {
        ShipInventorySlot existingSlot = itemSlots.Find(slot => slot.item == item);
        return existingSlot != null && existingSlot.quantity >= quantity;
    }

    public void RemoveItemFromShip(Item item, int quantity)
    {
        ShipInventorySlot existingSlot = itemSlots.Find(slot => slot.item == item);

        if (existingSlot != null)
        {
            existingSlot.quantity -= quantity;

            if (existingSlot.quantity <= 0)
            {
                itemSlots.Remove(existingSlot);
            }
        }
    }
}

[System.Serializable]
public class ShipInventorySlot
{
    public Item item;
    public int quantity;

    public ShipInventorySlot(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
