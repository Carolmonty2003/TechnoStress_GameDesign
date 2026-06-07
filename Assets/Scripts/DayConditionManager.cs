using UnityEngine;
using UnityEngine.UI;

public class DayConditionManager : MonoBehaviour
{
    public static DayConditionManager Instance { get; private set; }

    [Header("Umbral de tareas para 'Estresada'")]
    [SerializeField] private int taskOverloadThreshold = 6;

    [Header("Penalizaciones de stats")]
    [SerializeField] private float midnightStress    = 15f;
    [SerializeField] private float midnightFatigue   = 20f;
    [SerializeField] private float overloadStress    = 10f;
    [SerializeField] private float saturationStress  = 25f;
    [SerializeField] private float saturationFatigue = 15f;

    [Header("Fondo de escena")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite bgNormal;
    [SerializeField] private Sprite bgStressed;
    [SerializeField] private Sprite bgSaturation;
    [SerializeField] private Sprite bgExtremeFatigue;

    private int  _consecutiveStressedDays = 0;
    private bool _midnightApplied         = false; 

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        PhaseController.Instance.OnDayEnded  += OnDayEnded;
        EventManager.Instance.OnDayLoaded    += OnDayLoaded;
    }

    private void OnDestroy()
    {
        if (PhaseController.Instance != null)
            PhaseController.Instance.OnDayEnded -= OnDayEnded;
        if (EventManager.Instance != null)
            EventManager.Instance.OnDayLoaded  -= OnDayLoaded;
    }

    public void NotifyMidnightWork()
    {
        if (_midnightApplied) return;
        _midnightApplied = true;

        PlayerStats.Instance.ApplyChanges(
            stress:        midnightStress,
            digitalFatigue: midnightFatigue
        );

        SetBackground(bgExtremeFatigue);
        Debug.Log("[DayConditionManager] Fatiga Extrema: trabajando pasada la medianoche.");
    }


    private void OnDayEnded()
    {
        _midnightApplied = false; 
    }

    private void OnDayLoaded()
    {
        int taskCount = EventManager.Instance.EventsLeft.Count;

        if (taskCount > taskOverloadThreshold)
        {
            _consecutiveStressedDays++;

            if (_consecutiveStressedDays >= 2)
            {
                PlayerStats.Instance.ApplyChanges(
                    stress:        saturationStress,
                    digitalFatigue: saturationFatigue
                );
                SetBackground(bgSaturation);
                Debug.Log($"[DayConditionManager] Saturación: {_consecutiveStressedDays} días consecutivos sobrecargada ({taskCount} tareas).");
            }
            else
            {
                PlayerStats.Instance.ApplyChanges(stress: overloadStress);
                SetBackground(bgStressed);
                Debug.Log($"[DayConditionManager] Estresada: {taskCount} tareas en cola.");
            }
        }
        else
        {
            _consecutiveStressedDays = 0;
            SetBackground(bgNormal);
        }
    }


    private void SetBackground(Sprite sprite)
    {
        if (backgroundImage != null && sprite != null)
            backgroundImage.sprite = sprite;
    }
}
