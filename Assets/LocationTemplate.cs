using UnityEngine;
using TMPro;

public class LocationTemplate : MonoBehaviour
{
    public TextMeshProUGUI locationNameText;

    public void SetLocationInfo(string locationName)
    {
        locationNameText.text = locationName;

    }
}
