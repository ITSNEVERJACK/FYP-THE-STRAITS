using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipSelectTemplate : MonoBehaviour
{
    public TMP_Text shipNameText;
    public Image shipImage;

    private ShipData shipData;
    private ShipSelectionPanel shipSelectionPanel;

    public void Initialize(ShipData data, ShipSelectionPanel selectionPanel)
    {
        shipData = data;
        shipSelectionPanel = selectionPanel;

        shipNameText.text = shipData.GivenName;
        shipImage.sprite = shipData.shipImage;
    }
    
    public void OnButtonClick()
    {
        if (shipSelectionPanel != null)
        {
            shipSelectionPanel.OnShipSelected(shipData);
        }
    }
}
