using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName = "New Item";
    public string description = "Item Description";
    public Sprite icon = null;
    public int basePrice = 10;
    public int initialQuantity = 10;

    [HideInInspector]
    public int currentPrice;

    [HideInInspector]
    public float timeSinceLastPurchase;


}