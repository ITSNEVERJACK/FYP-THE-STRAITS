using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InvShipTemplate : MonoBehaviour
{
    public TMP_Text shipNameText;
    public Image shipImage;

    private ShipData shipData;
    private InvShipGenerator shipGenerator;

    public void Initialize(ShipData data, InvShipGenerator shipGen)
    {
        shipData = data;
        shipGenerator = shipGen;

        shipNameText.text = shipData.GivenName;
        shipImage.sprite = shipData.shipImage;
    }

    public void OnButtonClick()
    {
        if (shipGenerator != null)
        {
            shipGenerator.OnShipSelected(shipData);
        }
    }
}
