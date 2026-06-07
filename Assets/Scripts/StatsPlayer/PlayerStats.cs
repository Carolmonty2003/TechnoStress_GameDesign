using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    
    public float StartStress => startStress;
    public float StartFocus => startFocus;
    public float StartAnxiety => startAnxiety;
    public float StartPhysicalHealth => startPhysicalHealth;
    public float StartAcademicProgress => startAcademicProgress;
    public float StartDigitalFatigue => startDigitalFatigue;

    [Header("UI Mood")]
    [SerializeField] private TMP_Text moodText;
    [SerializeField] private MoodTips moodTips;

    // 2. A�ade la referencia a la imagen de la UI y los sprites que usar�s
    [SerializeField] private Image moodImage;
    [SerializeField] private Sprite moodSaturacionSprite;
    [SerializeField] private Sprite moodCansadaSprite;
    [SerializeField] private Sprite moodNormalSprite;
    [SerializeField] private Sprite moodEstresadaSprite;
    [SerializeField] private Sprite moodEnfocadaSprite;

    [Header("Thresholds")]
    [SerializeField] private float stressGameOver = 100f;
    [SerializeField] private float fatigueGameOver = 100f;
    [SerializeField, Range(0, 100)] private float fatigueForceSleep = 80f;
    [SerializeField] private float academicProgressToWin = 60f;

    public Stat Stress { get; private set; }
    public Stat Focus { get; private set; }
    public Stat Anxiety { get; private set; }
    public Stat PhysicalHealth { get; private set; }
    public Stat AcademicProgress { get; private set; }
    public Stat DigitalFatigue { get; private set; }

    public bool FatiguePunishment = false;
    private StatEvents _events => StatEvents.Instance;


    public string Mood
    {
        get
        {
            if (this.Stress.Value >= 90) return "Saturacion";
            else if (this.Stress.Value >= 60) return "Estresada";
            else if (this.Stress.Value >= 30) return "Normal";
            else return "Relajada";
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _events.OnForceSleep += () =>
        {
            PhaseController.Instance.Restart();
        };

        ResetStats();
    }

    private void Update()
    {
        moodText.text = "Mood: " + Mood;

        if (moodImage != null)
        {
            if (this.Stress.Value >= 90)
            {
                moodImage.sprite = moodSaturacionSprite;
                moodTips.LoadTip(MoodTips.MoodType.SATURATED);
            }
            else if (this.Stress.Value >= 60)
            {
                moodImage.sprite = moodEstresadaSprite;
                moodTips.LoadTip(MoodTips.MoodType.STRESSED);
            }
            else if (this.Stress.Value >= 30)
            {
                moodImage.sprite = moodCansadaSprite;
                moodTips.LoadTip(MoodTips.MoodType.TIRED);
            }
            else
            {
                moodImage.sprite = moodNormalSprite;
                moodTips.LoadTip(MoodTips.MoodType.NORMAL);
            }
        }
    }


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
        float prevFatigue = DigitalFatigue.Value;

        Stress.Add(stress);
        Focus.Add(focus);
        Anxiety.Add(anxiety);
        PhysicalHealth.Add(physicalHealth);
        AcademicProgress.Add(academicProgress);
        DigitalFatigue.Add(digitalFatigue);

        BroadcastAll();

        if (Stress.CrossedAbove(stressGameOver, prevStress) || DigitalFatigue.CrossedAbove(fatigueGameOver, prevFatigue))
            _events.GameOver();

        if (DigitalFatigue.CrossedAbove(fatigueForceSleep, prevFatigue)) 
            _events.ForceSleep();
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
