using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ItemManager : MonoBehaviour
{
    public int currency;
    public int labor;
    public int ships;

    private int currencyCap = 999999;
    private int laborCap = 999;
    private int shipsCap = 999;

    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI laborText;
    public TextMeshProUGUI shipsText;

    public List<InventoryItemSlot> itemSlots;
    public List<ShipData> shipsList;

    public int slotSize = 50;
    public int inventorySize = 50;

    private ItemDisplayManager itemDisplayManager;

    public delegate void ItemsRemovedHandler(List<Item> removedItems);
    public static event ItemsRemovedHandler OnItemsRemoved;

    private void Awake()
    {
        UpdateDisplays();
    }

    private void Start()
    {
        shipsList = new List<ShipData>();
        itemDisplayManager = FindObjectOfType<ItemDisplayManager>();
        GenerateInitialDisplays();
        UpdateDisplays();
    }

    public void UpdateDisplays()
    {
        if (itemDisplayManager != null)
        {
            itemDisplayManager.GenerateDisplays();
        }
        else
        {
            Debug.LogWarning("ItemDisplayManager is null. Make sure to set it using SetDisplayManager or set it in the inspector.");
        }
    }

    public void AddCurrency(int amount)
    {
        currency = Mathf.Min(currency + amount, currencyCap);
        UpdateResourceTexts();
        UpdateDisplays();
    }

    public void AddLabor(int amount)
    {
        labor = Mathf.Min(labor + amount, laborCap);
        UpdateResourceTexts();
        UpdateDisplays();
    }

    public void AddShip(ShipData shipData)
    {
        shipsList.Add(shipData);
        ships++;
        UpdateResourceTexts();
        UpdateDisplays();
    }

    public int GetShipAmount(ShipData shipData)
    {
        return shipsList.Count(s => s == shipData);
    }

    public int GetItemQuantity(Item item)
    {
        if (itemSlots == null)
        {
            return 0;
        }
        return itemSlots.FindAll(slot => slot.item == item).Sum(slot => slot.quantity);
    }

    public void AddItem(Item item, int quantity = 1)
    {
        if (itemSlots == null)
        {
            itemSlots = new List<InventoryItemSlot>();
        }

        InventoryItemSlot existingSlot = itemSlots.Find(slot => slot.item == item && slot.quantity < slotSize);

        if (existingSlot != null)
        {
            existingSlot.quantity += quantity;
        }
        else
        {
            if (itemSlots.Count < inventorySize)
            {
                itemSlots.Add(new InventoryItemSlot(item, quantity));
            }
            else
            {
                Debug.LogWarning("Reached the cap for item slots");
            }
        }

        UpdateResourceTexts();
        UpdateDisplays();
    }

    public void RemoveShip(ShipData shipData)
    {
        if (shipsList.Contains(shipData))
        {
            shipsList.Remove(shipData);
            ships--;
            UpdateResourceTexts();
            UpdateDisplays();
        }
        else
        {
            Debug.LogWarning("Attempted to remove a ship that does not exist in the shipsList.");
        }
    }

    public void RemoveItem(Item item, int quantity = 1)
    {
        List<Item> removedItems = new List<Item>();

        if (itemSlots != null)
        {
            InventoryItemSlot existingSlot = itemSlots.Find(slot => slot.item == item);

            if (existingSlot != null)
            {
                existingSlot.quantity = Mathf.Max(0, existingSlot.quantity - quantity);

                if (existingSlot.quantity == 0)
                {
                    itemSlots.Remove(existingSlot);
                    removedItems.Add(item);
                }

                UpdateResourceTexts();
                UpdateDisplays();
            }
            else
            {
                Debug.LogWarning("Attempted to remove an item that does not exist in the itemSlots.");
            }
        }
        else
        {
            Debug.LogError("itemSlots is null!");
        }

        OnItemsRemoved?.Invoke(removedItems);
    }

    public void SetSlotSize(int newSize)
    {
        slotSize = newSize;
    }

    [System.Serializable]
    public class InventoryItemSlot
    {
        public Item item;
        public int quantity;

        public InventoryItemSlot(Item item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }
    }

    public List<InventoryItemSlot> GetItemSlots()
    {
        return itemSlots;
    }

    public List<ShipData> GetShipsList()
    {
        return this.shipsList;
    }

    public void UpdateResourceTexts()
    {
        if (itemSlots != null)
        {
            currencyText.text = " " + currency;
            laborText.text = " " + labor;
            shipsText.text = " " + ships;
        }
        else
        {
            Debug.LogError("itemSlots is null!");
        }
    }

    private void GenerateInitialDisplays()
    {
        if (itemDisplayManager != null)
        {
            itemDisplayManager.GenerateDisplays();
        }
        else
        {
            Debug.LogWarning("ItemDisplayManager is null. Make sure to set it using SetDisplayManager or set it in the inspector.");
        }
    }
}
