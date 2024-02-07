using UnityEngine;

public class ItemSelectionManager : MonoBehaviour
{
    public GameObject itemSlotPrefab;
    public Transform contentPanel;
    public ItemManager itemManager;
    public ItemSelectionPanel itemSelectionPanelPrefab;

    private ItemSelectionPanel itemSelectionPanel;
    public static ItemSelectionManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }

    private void Start()
    {
        GenerateItemSelection();
    }

    private void GenerateItemSelection()
    {
        if (itemManager == null || itemSlotPrefab == null || contentPanel == null)
        {
            Debug.LogError("ItemManager reference, ItemSlot prefab, or ContentPanel transform is not set in the Inspector!");
            return;
        }

        foreach (ItemManager.InventoryItemSlot itemSlotData in itemManager.GetItemSlots())
        {
            GameObject itemSlotGO = Instantiate(itemSlotPrefab, contentPanel);
            ItemSlotUI itemSlotUI = itemSlotGO.GetComponent<ItemSlotUI>();

            itemSlotUI.SetItemData(itemSlotData, this); // Pass reference to ItemSelectionManager

            // No need to handle button click here, it's done in the ItemSlotUI script
        }

        itemSlotPrefab.SetActive(false);
    }

    public void OpenItemSelectPanel(ItemManager.InventoryItemSlot itemSlotData)
    {
        if (itemSelectionPanel == null)
        {
            itemSelectionPanel = Instantiate(itemSelectionPanelPrefab);
        }

        itemSelectionPanel.Open(itemSlotData.item, itemSlotData.quantity, this); // Pass reference to ItemSelectionManager
        itemSelectionPanel.gameObject.SetActive(true);
    }

    public void ConfirmItemSelection(Item item, int quantity)
    {
        itemManager.RemoveItem(item, quantity); // Remove the item from the ItemManager
        itemSelectionPanel.gameObject.SetActive(false);
    }
}
