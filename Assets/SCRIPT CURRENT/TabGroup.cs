using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<UiTabButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public UiTabButton selectedTab;
    public List<GameObject> objectToSwap;

    public void Subcribe(UiTabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<UiTabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(UiTabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.sprite = tabHover;
        }
    }

    public void OnTabExit(UiTabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(UiTabButton button)
    {
        int index = button.transform.GetSiblingIndex();

        if (selectedTab == button)
        {
            // If the selected tab is already active, deactivate it
            if (objectToSwap[index].activeSelf)
            {
                objectToSwap[index].SetActive(false);
            }
            else
            {
                // If the selected tab is not active, activate it
                objectToSwap[index].SetActive(true);
            }
        }
        else
        {
            selectedTab = button;
            ResetTabs();
            button.background.sprite = tabActive;

            for (int i = 0; i < objectToSwap.Count; i++)
            {
                if (i == index)
                {
                    objectToSwap[i].SetActive(true);
                }
                else
                {
                    objectToSwap[i].SetActive(false);
                }
            }
        }
    }

    public void ResetTabs()
    {
        foreach (UiTabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) { continue; }
            button.background.sprite = tabIdle;
        }
    }
}
