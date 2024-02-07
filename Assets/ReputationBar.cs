using UnityEngine;
using UnityEngine.UI;

public class ReputationBar : MonoBehaviour
{
    public Slider reputationSlider;
    public Slider favorabilitySlider;

    // Set the initial values
    public float initialReputationValue = 50f;
    public float initialFavorabilityValue = 50f;

    private void Start()
    {
        // Initialize sliders with initial values
        reputationSlider.value = initialReputationValue;
        favorabilitySlider.value = initialFavorabilityValue;
    }

    // Method to reduce reputation value
    public void ReduceReputation(float amount)
    {
        SetReputation(reputationSlider.value - amount);
    }

    // Method to increase reputation value
    public void IncreaseReputation(float amount)
    {
        SetReputation(reputationSlider.value + amount);
    }

    // Method to set reputation value directly
    public void SetReputation(float value)
    {
        reputationSlider.value = Mathf.Clamp(value, 0f, 100f);
    }

    // Method to reduce favorability value
    public void ReduceFavorability(float amount)
    {
        SetFavorability(favorabilitySlider.value - amount);
    }

    // Method to increase favorability value
    public void IncreaseFavorability(float amount)
    {
        SetFavorability(favorabilitySlider.value + amount);
    }

    // Method to set favorability value directly
    public void SetFavorability(float value)
    {
        favorabilitySlider.value = Mathf.Clamp(value, 0f, 100f);
    }
}
