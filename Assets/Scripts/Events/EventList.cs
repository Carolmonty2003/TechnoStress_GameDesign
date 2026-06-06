using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventList : MonoBehaviour
{
    [SerializeField] private TMP_Text[] texts;
    [SerializeField] private TMP_Text[] hourTexts;
    [SerializeField] private EventUI eventUI;

    void Start()
    {
        LoadEventList();
        eventUI.OnEventResolved += LoadEventList;
        PhaseController.Instance.OnTimeSpent += LoadEventList;
    }

    public void LoadEventList()
    {
        List<BaseEventData> events = EventManager.Instance.EventsLeft;
        int currentHour = PhaseController.Instance.CurrentHour;

        for (int i = 0; i < texts.Length; i++) texts[i].text = "";
        if (hourTexts != null)
            for (int i = 0; i < hourTexts.Length; i++) hourTexts[i].text = "";

        int slot = 0;
        for (int i = 0; i < events.Count && slot < texts.Length; i++)
        {
            if (events[i].scheduledHour > currentHour) continue;
            texts[slot].text = events[i].Name;
            if (hourTexts != null && slot < hourTexts.Length)
                hourTexts[slot].text = events[i].scheduledHour + ":00";
            slot++;
        }
    }
}

