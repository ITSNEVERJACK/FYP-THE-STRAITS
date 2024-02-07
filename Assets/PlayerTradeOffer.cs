using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class PlayerTradeOffer : MonoBehaviour
{
    public EnhancedBarterSystem barterSystem;
    public Image playerOfferImage;
    public TextMeshProUGUI playerOfferText;
    public Button assignOfferButton;
    public Button performTradeButton;

    public float resetTime = 1800.0f;
    private float timeSinceLastReset;

    private bool isPanelActive = true;

    [Serializable]
    public class PlayerOfferSlot
    {
        public Item item;
        public int quantity;

        public PlayerOfferSlot(Item item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }
    }

    [SerializeField]
    private List<PlayerOfferSlot> playerOfferSlots = new List<PlayerOfferSlot>();

    [SerializeField] // Expose isPlayerOfferSet in the Unity Inspector
    private bool isPlayerOfferSet = false;

    public bool IsPlayerOfferSet
    {
        get { return isPlayerOfferSet; }
    }

    private static PlayerTradeOffer instance;

    public static PlayerTradeOffer Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timeSinceLastReset = resetTime;
        UpdatePlayerOfferUI();

        ItemEventHandler.Instance.OnItemSelected += OnItemSelected;

        assignOfferButton.onClick.AddListener(() => SetPlayerOfferSlot(null, 0));
        performTradeButton.onClick.AddListener(PerformTrade);

        performTradeButton.interactable = false;
    }

    private void OnDestroy()
    {
        ItemEventHandler.Instance.OnItemSelected -= OnItemSelected;
    }

    private void OnItemSelected(object sender, ItemEventHandler.ItemSelectedEventArgs e)
    {
        if ((object)sender == (object)ItemEventHandler.Instance && e.item != null && isPanelActive)
        {
            SetPlayerOfferSlot(e.item, e.quantity);
        }
    }

    private void Update()
    {
        timeSinceLastReset += Time.deltaTime;

        if (timeSinceLastReset >= resetTime)
        {
            ResetOffer();
        }

        // Enable the perform trade button if there's at least one item in the player's offer slots
        performTradeButton.interactable = playerOfferSlots.Count > 0;
    }

    public void SetPlayerOfferSlot(Item item, int quantity)
    {
        PlayerOfferSlot existingSlot = playerOfferSlots.Find(slot => slot.item == item);

        if (existingSlot != null)
        {
            existingSlot.quantity += quantity;
        }
        else
        {
            playerOfferSlots.Add(new PlayerOfferSlot(item, quantity));
        }

        UpdatePlayerOfferUI();
        timeSinceLastReset = 0.0f;
        isPlayerOfferSet = true; // Set the flag to true when an offer slot is set
    }

    private void UpdatePlayerOfferUI()
    {
        if (barterSystem != null && playerOfferImage != null && playerOfferText != null)
        {
            if (playerOfferSlots.Count > 0)
            {
                PlayerOfferSlot latestSlot = playerOfferSlots[playerOfferSlots.Count - 1];

                if (latestSlot != null && latestSlot.item != null)
                {
                    playerOfferImage.sprite = latestSlot.item.icon;
                    playerOfferText.text = $"{latestSlot.item.itemName} x{latestSlot.quantity}";
                }
                else
                {
                    Debug.LogWarning("Latest offer slot or its item is null.");
                }
            }
            else
            {
                playerOfferImage.sprite = null;
                playerOfferText.text = "No Offer";
            }
        }
        else
        {
            Debug.LogWarning("One or more UI components are null.");
        }
    }

    private void ResetOffer()
    {
        barterSystem.SwitchTradeItem(null);
        UpdatePlayerOfferUI();
        ItemEventHandler.Instance.OnItemSelected -= OnItemSelected;
        isPlayerOfferSet = false; // Reset the flag when the offer is reset
    }

    public void DisablePanel()
    {
        isPanelActive = false;
        ItemEventHandler.Instance.OnItemSelected -= OnItemSelected;
    }

    public void PerformTrade()
    {
        if (isPlayerOfferSet)
        {
            // Gather the offer slots from the player
            List<PlayerOfferSlot> playerOfferSlotsList = playerOfferSlots;

            // Call the MakeTrade method in EnhancedBarterSystem with the player offer slots and an empty list for trader offer slots, along with the default playerReputation value
            barterSystem.MakeTrade(playerOfferSlotsList, new List<EnhancedBarterSystem.TraderOfferSlot>(), 0.0f);

            // Reset the player offer status
            isPlayerOfferSet = false;
        }
    }


}
