using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PriceTrackerTemplate : MonoBehaviour
{
    public Image itemImage;
    public TMP_Text priceText;
    public Image upArrowImage; 
    public Image downArrowImage; 
    public bool showUpArrow; 
    public bool showDownArrow; 
    private Sprite upArrowSprite; 
    private Sprite downArrowSprite; 

    private int previousPrice; 

    public void SetItemData(Item itemData)
    {
       
        if (itemImage != null)
        {
            itemImage.sprite = itemData.icon;
        }

        
        if (priceText != null)
        {
            priceText.text = "Price: " + itemData.basePrice;
        }

        
        previousPrice = itemData.basePrice;
    }

    
    public void SetPriceText(string newText)
    {
        if (priceText != null)
        {
            priceText.text = newText;
        }
    }

    
    public int GetPreviousPrice()
    {
        return previousPrice;
    }

    
    public void SetPreviousPrice(int newPreviousPrice)
    {
        previousPrice = newPreviousPrice;
    }

    
    public void UpdateArrowVisibility()
    {
        if (upArrowImage != null)
        {
            upArrowImage.enabled = showUpArrow;
        }

        if (downArrowImage != null)
        {
            downArrowImage.enabled = showDownArrow;
        }

        
        if (showUpArrow)
        {
            upArrowImage.enabled = true;
            downArrowImage.enabled = false;
        }
        else if (showDownArrow)
        {
            downArrowImage.enabled = true;
            upArrowImage.enabled = false;
        }
    }


    
    public void SetArrowSprite(Sprite upArrowSprite, Sprite downArrowSprite)
    {
        if (upArrowImage != null)
        {
            upArrowImage.sprite = upArrowSprite; 
            upArrowImage.enabled = false; 
        }

        if (downArrowImage != null)
        {
            downArrowImage.sprite = downArrowSprite; 
            downArrowImage.enabled = false; 
        }

        this.upArrowSprite = upArrowSprite;
        this.downArrowSprite = downArrowSprite;
    }
}
