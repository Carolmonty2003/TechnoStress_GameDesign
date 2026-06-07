using System.Collections.Generic;
using UnityEngine;
using Utils;

public class EventManager : Singleton<EventManager>
{
    [SerializeField] private EventUI eventUI;
    [SerializeField] private SortingEventUI sortingEventUI;
    [SerializeField] private MinigameController minigameController;
    [SerializeField] private List<BaseEventData> events = new();
    public List<BaseEventData> EventsLeft
    {
        get
        {
            int safeIndex = Mathf.Clamp(currentIndex, 0, events.Count);
            return events.GetRange(safeIndex, events.Count - safeIndex);
        }
    }

    [SerializeField] private List<DayData> days = new();

    private List<BaseEventData> events = new();
    public List<BaseEventData> EventsLeft => events.GetRange(currentIndex, events.Count - currentIndex);

    private int currentIndex = 0;
    private int currentDay = 0;
    private bool _waiting = false;

    [SerializeField] PlayerStats playerStats;

    public System.Action OnDayLoaded;

    private void Awake()
    {
        InitSingleton();
    }

    private void Start()
    {
        eventUI.OnEventResolved += () => { currentIndex++; };
        eventUI.OnEventResolved += ShowNextEvent;
        if (sortingEventUI != null)
        {
            sortingEventUI.OnEventResolved += () => { currentIndex++; };
            sortingEventUI.OnEventResolved += ShowNextEvent;
        if (minigameController != null)
        {
            minigameController.OnEventResolved += () => { currentIndex++; };
            minigameController.OnEventResolved += ShowNextEvent;
        }
        ShowNextEvent();
    }

    private void AdvanceDay()
    {
        PhaseDone();
        currentDay++;

        if (currentDay >= days.Count)
        {
            OnAllDaysDone();
            return;
        }

        LoadDay(currentDay);
    }

    private void ShowNextEvent()
    {
        if (currentIndex >= events.Count)
        {
            OnAllEventsDone();
            return;
        }

        BaseEventData currentEvent = events[currentIndex];

        Debug.Log($"[EventManager] ShowNextEvent → index={currentIndex} evento='{currentEvent.Name}' scheduledHour={currentEvent.scheduledHour} CurrentHour={PhaseController.Instance.CurrentHour} _waiting={_waiting}");

        if (PhaseController.Instance.CurrentHour < currentEvent.scheduledHour)
        {
            if (!_waiting)
            {
                _waiting = true;
                PhaseController.Instance.OnTimeSpent += OnTimeAdvanced;
                Debug.Log($"[EventManager] Esperando hasta las {currentEvent.scheduledHour}h (ahora son las {PhaseController.Instance.CurrentHour}h)");
            }
            return;
        }

        if (_waiting)
        {
            PhaseController.Instance.OnTimeSpent -= OnTimeAdvanced;
            _waiting = false;
        }

        Debug.Log($"[EventManager] Mostrando evento '{currentEvent.Name}' a las {PhaseController.Instance.CurrentHour}h (scheduledHour={currentEvent.scheduledHour})");

        if (currentEvent is EventData choiceEvent)
        {
            eventUI.ShowEvent(choiceEvent);
        }
        else if (currentEvent is SortingEventData sortingEvent)
        {
            if (sortingEventUI != null)
                sortingEventUI.ShowEvent(sortingEvent);
            else
            {
                currentIndex++;
                ShowNextEvent();
            }
        }
        else if (currentEvent is MinigameEventData)
        {
            if (minigameController != null)
                minigameController.LaunchMinigame();
            else
            {
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

    private void OnAllEventsDone()
    {
        PhaseController.Instance.AllEventsDone();
        PhaseController.Instance.EndDay();
    }

    private void OnAllDaysDone()
    {
        Debug.Log("All days done");
    }

    public void PhaseDone()
    {
        if (currentIndex >= events.Count) return;

        float stress = 0, focus = 0, anxiety = 0, physicalHealth = 0, academicProgress = 0, digitalFatigue = 0;
        for (int i = currentIndex; i < events.Count; i++)
        {
            stress          += events[i].stress;
            focus           += events[i].focus;
            anxiety         += events[i].anxiety;
            physicalHealth  += events[i].physicalHealth;
            academicProgress+= events[i].academicProgress;
            digitalFatigue  += events[i].digitalFatigue;
        }
        playerStats.ApplyChanges(stress, focus, anxiety, physicalHealth, academicProgress, digitalFatigue);
    }
}

