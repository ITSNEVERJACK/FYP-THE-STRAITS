using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopTemplate : MonoBehaviour
{
    public TMP_Text NameText;
    public TMP_Text DescriptionText;
    public TMP_Text Pricetext;
    public TMP_Text OwnedText;
    public TMP_Text AvailabilityText;
    public Image ItemImage;

    private ItemManager itemManager; // Private field to hold the reference
    private Item item; // Private field to hold the associated item

    public void SetItem(Item itemData)
    {
        item = itemData;
    }

    public Item GetItem()
    {
        return item;
    }

    public ItemManager ItemManagerReference
    {
        set { itemManager = value; }
    }

    public void UpdateQuantityText(Item itemData)
    {
        if (itemManager != null)
        {
            int quantityOwned = itemManager.GetItemQuantity(itemData); // Updated line
            OwnedText.text = "Owned: " + quantityOwned;
        }

    }

    public void SetAvailableQuantity(int quantity)
    {
        if (AvailabilityText != null)
        {
            AvailabilityText.text = "Available: " + quantity;
        }
    }

    public void UpdatePriceAndAvailabilityText(int price, int availability)
    {
        Pricetext.text = "Price: " + price;
        AvailabilityText.text = "Available: " + availability;
    }
}
