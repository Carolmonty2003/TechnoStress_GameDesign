using System.Collections.Generic;
using UnityEngine;

// holds the list of events for the day and shows them one by one
public class EventManager : MonoBehaviour
{
    [SerializeField] private EventUI eventUI;
    [SerializeField] private SortingEventUI sortingEventUI;
    [SerializeField] private List<BaseEventData> events = new();

    private int currentIndex = 0;

    [SerializeField] PlayerStats playerStats;

    private void Start()
    {
        eventUI.OnEventResolved += ShowNextEvent;
        if (sortingEventUI != null)
        {
            sortingEventUI.OnEventResolved += ShowNextEvent;
        }
        ShowNextEvent();
    }

    private void ShowNextEvent()
    {
        if (currentIndex >= events.Count)
        {
            OnAllEventsDone();
            return;
        }

        BaseEventData currentEvent = events[currentIndex];
        if (currentEvent is EventData choiceEvent)
        {
            eventUI.ShowEvent(choiceEvent);
        }
        else if (currentEvent is SortingEventData sortingEvent)
        {
            if (sortingEventUI != null)
            {
                sortingEventUI.ShowEvent(sortingEvent);
            }
            else
            {
                currentIndex++;
                ShowNextEvent();
                return;
            }
        }
        currentIndex++;
    }

    // called when all events of the day are finished
    private void OnAllEventsDone()
    {
        Debug.Log("All events done");
        // here you can call PhaseController, show day summary, etc
    }

    public void PhaseDone()
    {
        if (currentIndex < events.Count)
        {
            float stress = 0, focus = 0, anxiety = 0, physicalHealth = 0, academicProgress = 0, digitalFatigue = 0;
            for (int i = currentIndex; i < events.Count; i++)
            {
                stress += events[i].stress;
                focus += events[i].focus;
                anxiety += events[i].anxiety;
                physicalHealth += events[i].physicalHealth;
                academicProgress += events[i].academicProgress;
                digitalFatigue += events[i].digitalFatigue;
            }
            playerStats.ApplyChanges(stress, focus, anxiety, physicalHealth, academicProgress, digitalFatigue);
        }
    }
}
