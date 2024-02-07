// BuildingTemplate.cs
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingTemplate : MonoBehaviour
{
    public Image shipImage;
    public TextMeshProUGUI timerText;

    public void Initialize(ShipData shipData)
    {
        shipImage.sprite = shipData.shipImage;
        UpdateTimer(shipData.buildTime);
    }

    public void UpdateTimer(float timeLeft) => timerText.text = timeLeft.ToString("F1");
}
