using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemSelectionPanel : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TMP_InputField quantityInput;
    public Button confirmButton;
    public Button cancelButton;
    public Button increaseButton;
    public Button decreaseButton;

    private Item itemToSelect;
    private int currentQuantity = 1;
    private int availableQuantity = 0;
    private ItemSelectionManager selectionManager;

    private void Awake()
    {
        confirmButton.onClick.AddListener(ConfirmSelection);
        cancelButton.onClick.AddListener(CancelSelection);
        increaseButton.onClick.AddListener(IncreaseQuantity);
        decreaseButton.onClick.AddListener(DecreaseQuantity);
    }

    public void Open(Item item, int availableQty, ItemSelectionManager manager)
    {
        itemToSelect = item;
        availableQuantity = availableQty;
        selectionManager = manager;

        itemNameText.text = itemToSelect.name;

        currentQuantity = 1;
        quantityInput.text = currentQuantity.ToString();

        gameObject.SetActive(true);
    }

    private void ConfirmSelection()
    {
        int selectedQuantity = int.Parse(quantityInput.text);
        selectionManager.ConfirmItemSelection(itemToSelect, selectedQuantity); // Confirm the item selection to the ItemSelectionManager
    }

    private void CancelSelection()
    {
        gameObject.SetActive(false);
    }

    private void IncreaseQuantity()
    {
        currentQuantity++;
        UpdateQuantityText();
    }

    private void DecreaseQuantity()
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
}
