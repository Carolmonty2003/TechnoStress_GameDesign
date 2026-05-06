using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Starting values")]
    [SerializeField] private float startStress = 10f;
    [SerializeField] private float startFocus = 80f;
    [SerializeField] private float startAnxiety = 10f;
    [SerializeField] private float startPhysicalHealth = 90f;
    [SerializeField] private float startAcademicProgress = 50f;
    [SerializeField] private float startDigitalFatigue = 0f;

    [Header("Thresholds")]
    [SerializeField] private float stressGameOver = 100f;
    [SerializeField] private float academicProgressToWin = 60f;

    public Stat Stress { get; private set; }
    public Stat Focus { get; private set; }
    public Stat Anxiety { get; private set; }
    public Stat PhysicalHealth { get; private set; }
    public Stat AcademicProgress { get; private set; }
    public Stat DigitalFatigue { get; private set; }

    private StatEvents _events => StatEvents.Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start() => ResetStats();

    public void ResetStats()
    {
        Stress = new Stat(startStress);
        Focus = new Stat(startFocus);
        Anxiety = new Stat(startAnxiety);
        PhysicalHealth = new Stat(startPhysicalHealth);
        AcademicProgress = new Stat(startAcademicProgress);
        DigitalFatigue = new Stat(startDigitalFatigue);

        BroadcastAll();
    }

    public void ApplyChanges(float stress = 0, float focus = 0, float anxiety = 0,float physicalHealth = 0, float academicProgress = 0, float digitalFatigue = 0)
    {
        float prevStress = Stress.Value;

        Stress.Add(stress);
        Focus.Add(focus);
        Anxiety.Add(anxiety);
        PhysicalHealth.Add(physicalHealth);
        AcademicProgress.Add(academicProgress);
        DigitalFatigue.Add(digitalFatigue);

        BroadcastAll();

        if (Stress.CrossedAbove(stressGameOver, prevStress))
            _events.GameOver();
    }

    public void EndDay() => _events.DayEnded();

    public bool CanWin() =>
        AcademicProgress.Value >= academicProgressToWin &&
        Stress.Value < stressGameOver;

    private void BroadcastAll()
    {
        _events.StressChanged(Stress.Value);
        _events.FocusChanged(Focus.Value);
        _events.AnxietyChanged(Anxiety.Value);
        _events.PhysicalHealthChanged(PhysicalHealth.Value);
        _events.AcademicProgressChanged(AcademicProgress.Value);
        _events.DigitalFatigueChanged(DigitalFatigue.Value);
    }
}