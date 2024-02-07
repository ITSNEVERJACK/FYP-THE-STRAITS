using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemDisplayManager : MonoBehaviour
{
    public GameObject itemDisplayTemplatePrefab;
    public Transform itemDisplayParentTransform;

    [SerializeField]
    private ItemManager itemManager;

    private void OnEnable()
    {
        UpdateDisplays();
    }

    private void Start()
    {
        GenerateInitialDisplays();
    }

    public void UpdateDisplays()
    {
        if (itemManager != null)
        {
            GenerateDisplays();
        }
        else
        {
            Debug.LogError("ItemManager reference is not set in the inspector.");
        }
    }

    private void GenerateInitialDisplays()
    {
        if (itemManager == null)
        {
            Debug.LogError("ItemManager reference is not set in the inspector.");
            return;
        }

        ClearItems(itemDisplayParentTransform);
        itemDisplayTemplatePrefab.SetActive(true); // Re-enable the prefab before initial generation

        List<ItemManager.InventoryItemSlot> distinctSlots = itemManager.itemSlots.Distinct().ToList();

        foreach (var slot in distinctSlots)
        {
            GameObject itemDisplayInstance = Instantiate(itemDisplayTemplatePrefab, itemDisplayParentTransform);
            InvItem invItem = itemDisplayInstance.GetComponent<InvItem>();

            if (invItem != null)
            {
                invItem.SetItemImageAndQuantity(slot.item.icon, slot.quantity, slot.item.currentPrice);
            }
        }

        itemDisplayTemplatePrefab.SetActive(false); // Disable the prefab after initial generation
    }

    public void GenerateDisplays()
    {
        if (itemManager == null)
        {
            Debug.LogError("ItemManager reference is not set in the inspector.");
            return;
        }

        ClearItems(itemDisplayParentTransform);
        itemDisplayTemplatePrefab.SetActive(true); // Re-enable the prefab before generation
        List<ItemManager.InventoryItemSlot> distinctSlots = itemManager.itemSlots.Distinct().ToList();

        foreach (var slot in distinctSlots)
        {
            GameObject itemDisplayInstance = Instantiate(itemDisplayTemplatePrefab, itemDisplayParentTransform);
            InvItem invItem = itemDisplayInstance.GetComponent<InvItem>();

            if (invItem != null)
            {
                invItem.SetItemImageAndQuantity(slot.item.icon, slot.quantity, slot.item.currentPrice);
            }
        }

        itemDisplayTemplatePrefab.SetActive(false); // Disable the prefab after generation complete
    }

    private void ClearItems(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
