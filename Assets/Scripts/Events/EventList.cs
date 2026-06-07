using System.Collections;
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
        eventUI.OnEventResolved += () => StartCoroutine(RefreshNextFrame());
        PhaseController.Instance.OnTimeSpent += LoadEventList;
        EventManager.Instance.OnDayLoaded += LoadEventList;
    }

    private void OnDestroy()
    {
        if (PhaseController.Instance != null)
            PhaseController.Instance.OnTimeSpent -= LoadEventList;
    }

    private IEnumerator RefreshNextFrame()
    {
        yield return null;
        LoadEventList();
    }

    public void LoadEventList()
    {
        List<BaseEventData> events = new List<BaseEventData>(EventManager.Instance.EventsLeft);
        events.Sort((a, b) => EventManager.Instance.GetEventScheduledHour(a)
                                   .CompareTo(EventManager.Instance.GetEventScheduledHour(b)));

        for (int i = 0; i < texts.Length; i++) texts[i].text = "";
        if (hourTexts != null)
            for (int i = 0; i < hourTexts.Length; i++) hourTexts[i].text = "";

        for (int i = 0; i < events.Count && i < texts.Length; i++)
        {
            texts[i].text = events[i].Name;
            if (hourTexts != null && i < hourTexts.Length)
            {
                int h = EventManager.Instance.GetEventScheduledHour(events[i]) % 24;
                hourTexts[i].text = h + ":00";
            }
        }
    }
}

