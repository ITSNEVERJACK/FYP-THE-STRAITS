using UnityEngine;
using System;

public class ItemEventHandler : MonoBehaviour
{
    public static ItemEventHandler Instance;

    public class ItemSelectedEventArgs : EventArgs
    {
        public Item item;
        public int quantity;

        public ItemSelectedEventArgs(Item item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }
    }

    public event EventHandler<ItemSelectedEventArgs> OnItemSelected;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerItemSelected(Item item, int quantity)
    {
        OnItemSelected?.Invoke(this, new ItemSelectedEventArgs(item, quantity));
    }
}
