using UnityEngine;
using UnityEngine.EventSystems;

public class DisablePanelScript : MonoBehaviour, IPointerClickHandler
{
    public GameObject panelToDisable;

    void Start()
    {
        // Ensure that a panel is assigned in the Inspector
        if (panelToDisable == null)
        {
            Debug.LogError("Panel to disable is not assigned!");
            enabled = false;
            return;
        }

        // Attach this script as a click handler
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { DisablePanel(); });

        trigger.triggers.Add(entry);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Called when the button is clicked
        DisablePanel();
    }

    void DisablePanel()
    {
        // Disable the panel when the button is clicked
        if (panelToDisable != null)
        {
            panelToDisable.SetActive(false);
        }
    }
}
