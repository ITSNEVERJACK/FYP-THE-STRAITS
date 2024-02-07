using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuyItemUI : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;
    public TextMeshProUGUI availableQuantityText;
    public TMP_InputField quantityInput;
    public Button confirmButton;
    public Button cancelButton;
    public Button increaseButton;
    public Button decreaseButton;
    public Image itemImage; // Add an Image component to display the item image


    private Item itemToBuy;
    private ShopSystem shopSystem;
    private ShopTemplate shopTemplate;
    private int currentQuantity = 1;
    private int availableQuantity = 0;

    private static BuyItemUI instance;

    public static BuyItemUI Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
       
    }

    public void Open(Item item, ShopSystem shop, ShopTemplate shopTemplate, int availableQty)
    {
        itemToBuy = item;
        shopSystem = shop;
        this.shopTemplate = shopTemplate;
        availableQuantity = availableQty;

        itemNameText.text = itemToBuy.name;
        itemPriceText.text = "Price: " + itemToBuy.currentPrice;
        availableQuantityText.text = "Available: " + availableQuantity;

        currentQuantity = 1;
        quantityInput.text = currentQuantity.ToString();

        // Set the item image to the selected item's image
        itemImage.sprite = itemToBuy.icon;

        confirmButton.onClick.AddListener(ConfirmPurchase);
        cancelButton.onClick.AddListener(CancelPurchase);
        increaseButton.onClick.AddListener(IncreaseQuantity);
        decreaseButton.onClick.AddListener(DecreaseQuantity);

        gameObject.SetActive(true);
    }

    public int GetRequestedQuantity()
    {
        return currentQuantity;
    }

    public void ConfirmPurchase()
    {
        int quantity;
        if (int.TryParse(quantityInput.text, out quantity))
        {
            if (quantity > 0)
            {
                if (quantity <= availableQuantity)
                {
                    shopSystem.BuyItem(itemToBuy, shopTemplate, quantity);
                    CloseUI();
                }
                else
                {
                    Debug.LogWarning("Not enough available quantity to buy " + itemToBuy.name);
                }
            }
            else
            {
                Debug.LogWarning("Quantity must be greater than zero.");
            }
        }
        else
        {
            Debug.LogWarning("Invalid quantity input.");
        }
    }

    public void CancelPurchase()
    {
        CloseUI();
    }

    public void IncreaseQuantity()
    {
        currentQuantity++;
        UpdateQuantityText();
    }

    public void DecreaseQuantity()
    {
        if (currentQuantity > 1)
        {
            currentQuantity--;
            UpdateQuantityText();
        }
    }

    private void UpdateQuantityText()
    {
        quantityInput.text = currentQuantity.ToString();
    }
    private void CloseUI()
    {
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        increaseButton.onClick.RemoveAllListeners();
        decreaseButton.onClick.RemoveAllListeners();

        gameObject.SetActive(false);
    }
}
