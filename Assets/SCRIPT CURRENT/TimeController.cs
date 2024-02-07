using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Event
{
    public int year;
    public string eventName;
}

public class TimeController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI yearText;

    [SerializeField]
    private TextMeshProUGUI eventText;

    [SerializeField]
    private Button speedButton;

    [SerializeField]
    private List<Event> events;

    private int year = 1400;
    private float timeScale = 1f;
    private bool isPaused = false;

    private int speedIndex = 1; // Start with the 1-minute speed factor
    private float[] speedFactors = { 1f, 3f, 5f }; // Speed factors: 1 min per year, 3 min per year, 5 min per year

    private float timeCounter = 0f;
    private float timeCycleDuration = 60f; // 1 minute in seconds

    private string currentEvent = "";

    private void Awake()
    {
        // Initialize the UI text elements
        if (yearText != null)
            yearText.text = "Year: " + year;
        if (eventText != null)
            eventText.text = "";
        if (speedButton != null)
            UpdateSpeedButtonText();
    }

    void Update()
    {
        if (!isPaused)
        {
            // Update the time counter
            timeCounter += Time.deltaTime;

            // Check if it's time to cycle the year
            if (timeCounter >= timeCycleDuration)
            {
                timeCounter -= timeCycleDuration;
                IncreaseYear();
            }

            // Update the UI text element for the year
            if (yearText != null)
                yearText.text = "Year: " + year;

            // Check for events
            CheckEvents();
        }
    }

    void CheckEvents()
    {
        // Loop through the events and check if any match the current year
        foreach (Event e in events)
        {
            if (e.year == year)
            {
                if (eventText != null && currentEvent != e.eventName)
                {
                    eventText.text = e.eventName;
                    currentEvent = e.eventName;
                }
                break;
            }
        }
    }

    void IncreaseYear()
    {
        year++;
    }

    public void PauseTime()
    {
        isPaused = true;
    }

    public void PlayTime()
    {
        isPaused = false;
    }

    public void ChangeSpeed()
    {
        speedIndex++;
        if (speedIndex >= speedFactors.Length)
            speedIndex = 0;

        timeCycleDuration = 60f * speedFactors[speedIndex];
        UpdateSpeedButtonText();
    }

    void UpdateSpeedButtonText()
    {
        if (speedButton != null)
        {
            TextMeshProUGUI buttonText = speedButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
                buttonText.text = "Speed: " + speedFactors[speedIndex] + " min/yr";
        }
    }
}
