using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InvItem : MonoBehaviour
{
    public Image itemImage;
    public TMP_Text ownedText;

    private ItemManager itemManager;
    private Item item;

    public void SetItem(Item itemData, ItemManager manager)
    {
        item = itemData;
        itemManager = manager;
        UpdateUI();
    }

    public void SetItemImageAndQuantity(Sprite sprite, int quantity, int price)
    {
        itemImage.sprite = sprite;
        ownedText.text = " " + quantity;
        // You can use the price variable as needed.
    }

    private void UpdateUI()
    {
        if (item != null)
        {
            itemImage.sprite = item.icon;
        }
        else
        {
            Debug.LogWarning("Item reference in InvItem is null.");
        }

        UpdateQuantityText();
    }

    private void UpdateQuantityText()
    {
        if (itemManager != null)
        {
            int quantityOwned = itemManager.GetItemQuantity(item);
            ownedText.text = " " + quantityOwned;
        }
        else
        {
            Debug.LogWarning("ItemManager reference in InvItem is null.");
        }
    }
}
