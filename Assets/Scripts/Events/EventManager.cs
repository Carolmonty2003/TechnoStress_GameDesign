using System.Collections.Generic;
using UnityEngine;

// holds the list of events for the day and shows them one by one
public class EventManager : MonoBehaviour
{
    [SerializeField] private EventUI eventUI;
    [SerializeField] private List<EventData> events = new();

    private int currentIndex = 0;

    private void Start()
    {
        eventUI.OnEventResolved += ShowNextEvent;
        ShowNextEvent();
    }

    // shows current event, or does nothing if the list is done
    private void ShowNextEvent()
    {
        if (currentIndex >= events.Count)
        {
            OnAllEventsDone();
            return;
        }

        eventUI.ShowEvent(events[currentIndex]);
        currentIndex++;
    }

    // called when all events of the day are finished
    private void OnAllEventsDone()
    {
        Debug.Log("All events done");
        // here you can call PhaseController, show day summary, etc
    }
}
