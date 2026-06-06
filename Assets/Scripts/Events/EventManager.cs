using System.Collections.Generic;
using UnityEngine;
using Utils;

// holds the list of events for the day and shows them one by one
public class EventManager : Singleton<EventManager>
{
    [SerializeField] private EventUI eventUI;
    [SerializeField] private SortingEventUI sortingEventUI;
    [SerializeField] private MinigameController minigameController;
    [SerializeField] private List<BaseEventData> events = new();
    public List<BaseEventData> EventsLeft {  get { return events.GetRange(currentIndex, events.Count - currentIndex); } }

    private int currentIndex = 0;
    private bool _waiting = false;

    [SerializeField] PlayerStats playerStats;

    private void Awake()
    {
        InitSingleton();
        events.Sort((a, b) => a.scheduledHour.CompareTo(b.scheduledHour));
    }

    private void Start()
    {
        eventUI.OnEventResolved += () => { currentIndex++; };
        eventUI.OnEventResolved += ShowNextEvent;
        if (sortingEventUI != null)
        {
            sortingEventUI.OnEventResolved += ShowNextEvent;
        }
        if (minigameController != null)
        {
            minigameController.OnEventResolved += () => { currentIndex++; };
            minigameController.OnEventResolved += ShowNextEvent;
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

        if (PhaseController.Instance.CurrentHour < currentEvent.scheduledHour)
        {
            if (!_waiting)
            {
                _waiting = true;
                PhaseController.Instance.OnTimeSpent += OnTimeAdvanced;
            }
            return;
        }

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
        else if (currentEvent is MinigameEventData)
        {
            if (minigameController != null)
            {
                minigameController.LaunchMinigame();
            }
            else
            {
                // Si no hay controlador asignado, se salta el evento
                currentIndex++;
                ShowNextEvent();
            }
        }
    }

    private void OnTimeAdvanced()
    {
        if (PhaseController.Instance.CurrentHour >= events[currentIndex].scheduledHour)
        {
            PhaseController.Instance.OnTimeSpent -= OnTimeAdvanced;
            _waiting = false;
            ShowNextEvent();
        }
    }

    // called when all events of the day are finished
    private void OnAllEventsDone()
    {
        Debug.Log("All events done");
        // here you can call PhaseController, show day summary, etc
        PhaseController.Instance.AllEventsDone();
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
