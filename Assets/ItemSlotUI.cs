using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Image itemImage;
    private ItemManager.InventoryItemSlot inventoryItemSlot;
    private ItemSelectionManager itemSelectionManager;

    public void SetItemData(ItemManager.InventoryItemSlot itemSlot, ItemSelectionManager manager)
    {
        inventoryItemSlot = itemSlot;
        itemSelectionManager = manager;

        if (itemImage != null)
        {
            itemImage.sprite = itemSlot.item.icon;
        }

        // Add a click listener to the button
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners(); // Remove existing listeners
            button.onClick.AddListener(OnItemClick);
        }
    }

    public void OnItemClick()
    {
        // Check if the itemSelectionManager reference is valid
        if (itemSelectionManager != null)
        {
            // Open the item selection panel using the manager
            itemSelectionManager.OpenItemSelectPanel(inventoryItemSlot);
        }
        else
        {
            Debug.LogError("ItemSelectionManager reference is not set!");
        }
    }

    public ItemManager.InventoryItemSlot GetItemData()
    {
        return inventoryItemSlot;
    }
}
