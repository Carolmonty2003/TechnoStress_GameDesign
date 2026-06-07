using System.Collections.Generic;
using UnityEngine;
using Utils;

public class EventManager : Singleton<EventManager>
{
    [SerializeField] private EventUI eventUI;
    [SerializeField] private SortingEventUI sortingEventUI;
    [SerializeField] private MinigameController minigameController;

    [SerializeField] private List<DayData> days = new();

    private List<BaseEventData> events = new();
    public List<BaseEventData> EventsLeft
    {
        get
        {
            int safeIndex = Mathf.Clamp(currentIndex, 0, events.Count);
            return events.GetRange(safeIndex, events.Count - safeIndex);
        }
    }

    public int GetEventScheduledHour(BaseEventData e) => GetHour(e);

    private int currentIndex = 0;
    private int currentDay = 0;
    private bool _waiting = false;
    private bool _eventInProgress = false;
    private readonly Dictionary<BaseEventData, int> _hourOverrides = new();

    [SerializeField] PlayerStats playerStats;

    public System.Action OnDayLoaded;

    private void Awake()
    {
        InitSingleton();
    }

    private void Start()
    {
        eventUI.OnEventResolved += OnEventResolved;

        if (sortingEventUI != null)
            sortingEventUI.OnEventResolved += OnEventResolved;

        PhaseController.Instance.OnDayEnded += AdvanceDay;

        LoadDay(0);
    }

    private void LoadDay(int dayIndex)
    {
        if (_waiting)
        {
            PhaseController.Instance.OnTimeSpent -= OnTimeAdvanced;
            _waiting = false;
        }

        currentIndex = 0;
        _eventInProgress = false;
        _hourOverrides.Clear();
        events = new List<BaseEventData>(days[dayIndex].events);
        events.Sort((a, b) => GetHour(a).CompareTo(GetHour(b)));

        OnDayLoaded?.Invoke();
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

    private int GetHour(BaseEventData e) =>
        _hourOverrides.TryGetValue(e, out int h) ? h : e.scheduledHour;

    public void DeferCurrentEvent()
    {
        if (currentIndex >= events.Count) return;

        BaseEventData toDefer = events[currentIndex];

        int newHour;
        int lastIndex = events.Count - 1;

        if (lastIndex <= currentIndex)
        {
            newHour = PhaseController.Instance.CurrentHour + 1;
        }
        else
        {
            BaseEventData lastEvent = events[lastIndex];
            newHour = GetHour(lastEvent) + GetEventDurationHours(lastEvent);
        }

        _hourOverrides[toDefer] = newHour;

        events.RemoveAt(currentIndex);
        events.Add(toDefer);
        events.Sort((a, b) => GetHour(a).CompareTo(GetHour(b)));

        Debug.Log($"[EventManager] '{toDefer.Name}' diferido a las {newHour}h. Cola: {string.Join(", ", events.ConvertAll(e => $"{e.Name}({GetHour(e)}h)"))}");

        _eventInProgress = false;
        ShowNextEvent();
    }

    private int GetEventDurationHours(BaseEventData eventData)
    {
        if (eventData is EventData choiceEvent && choiceEvent.choices.Count > 0)
        {
            float maxMinutes = 0f;
            foreach (var c in choiceEvent.choices)
                maxMinutes = Mathf.Max(maxMinutes, c.timeSpent);
            return Mathf.Max(1, Mathf.RoundToInt(maxMinutes / 60f));
        }
        return 1;
    }

    private void OnEventResolved()
    {
        _eventInProgress = false;
        currentIndex++;
        ShowNextEvent();
    }

    private void ShowNextEvent()
    {
        if (_eventInProgress) return;

        if (currentIndex >= events.Count)
        {
            OnAllEventsDone();
            return;
        }

        BaseEventData currentEvent = events[currentIndex];

        if (PhaseController.Instance.CurrentHour < GetHour(currentEvent))
        {
            if (!_waiting)
            {
                _waiting = true;
                PhaseController.Instance.OnTimeSpent += OnTimeAdvanced;
            }
            return;
        }

        if (_waiting)
        {
            PhaseController.Instance.OnTimeSpent -= OnTimeAdvanced;
            _waiting = false;
        }

        _eventInProgress = true;

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
                _eventInProgress = false;
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
                _eventInProgress = false;
                currentIndex++;
                ShowNextEvent();
            }
        }
    }

    private void OnTimeAdvanced()
    {
        if (PhaseController.Instance.CurrentHour >= GetHour(events[currentIndex]))
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
            stress           += events[i].stress;
            focus            += events[i].focus;
            anxiety          += events[i].anxiety;
            physicalHealth   += events[i].physicalHealth;
            academicProgress += events[i].academicProgress;
            digitalFatigue   += events[i].digitalFatigue;
        }
        playerStats.ApplyChanges(stress, focus, anxiety, physicalHealth, academicProgress, digitalFatigue);
    }
}
