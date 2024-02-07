using UnityEngine;
using TMPro;
using UnityEngine.UI; // Added using directive for UI components

public class ItemDisplayTemplate : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemQuantityText;
    public TextMeshProUGUI itemPriceText;
    public Image itemImage; // Added reference to the Image component

    public void SetItemInfo(Item item, int quantity, int price)
    {
        itemNameText.text = item.itemName;
        itemQuantityText.text = "Quantity: " + quantity.ToString();
        itemPriceText.text = "Price: " + price.ToString();
    }

    // New method to set the item image
    public void SetItemImage(Sprite icon)
    {
        if (itemImage != null)
        {
            itemImage.sprite = icon;
        }
        else
        {
            Debug.LogError("ItemDisplayTemplate is missing a reference to the Image component for item images.");
        }
    }
}
