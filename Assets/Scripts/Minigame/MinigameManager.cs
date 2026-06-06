using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controla el estado del minijuego: temporizador de 30 segundos,
/// puntuación y eventos de inicio/fin.
/// </summary>
public class MinigameManager : MonoBehaviour
{
    // ── Singleton ────────────────────────────────────────────────────────────
    public static MinigameManager Instance { get; private set; }

    // ── Configuración ────────────────────────────────────────────────────────
    [Header("Configuración")]
    [Tooltip("Duración del minijuego en segundos.")]
    public float gameDuration = 30f;

    [Tooltip("Si está activo, el minijuego arranca solo al cargar la escena. Desactívalo cuando lo controla MinigameController.")]
    public bool autoStart = false;

    // ── Estado ───────────────────────────────────────────────────────────────
    public bool  IsPlaying    { get; private set; }
    public int   Score        { get; private set; }
    public float TimeLeft     { get; private set; }

    // ── Eventos ──────────────────────────────────────────────────────────────
    [Header("Eventos")]
    public UnityEvent          OnGameStart;
    public UnityEvent<int>     OnScoreChanged;
    public UnityEvent<float>   OnTimeChanged;
    public UnityEvent<int>     OnGameEnd;       // entrega la puntuación final

    // ── Referencias ──────────────────────────────────────────────────────────
    private CircleSpawner spawner;

    // ─────────────────────────────────────────────────────────────────────────
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        spawner = FindFirstObjectByType<CircleSpawner>();
        if (autoStart)
            StartGame();
    }

    void Update()
    {
        if (!IsPlaying) return;

        TimeLeft -= Time.deltaTime;
        TimeLeft  = Mathf.Max(TimeLeft, 0f);
        OnTimeChanged?.Invoke(TimeLeft);

        if (TimeLeft <= 0f)
        {
            EndGame();
        }
    }

    // ── Métodos públicos ─────────────────────────────────────────────────────

    /// <summary>Inicia o reinicia el minijuego.</summary>
    public void StartGame()
    {
        Score     = 0;
        TimeLeft  = gameDuration;
        IsPlaying = true;

        OnGameStart?.Invoke();
        OnScoreChanged?.Invoke(Score);
        OnTimeChanged?.Invoke(TimeLeft);
    }

    /// <summary>Suma puntos al marcador.</summary>
    public void AddScore(int amount)
    {
        if (!IsPlaying) return;
        Score += amount;
        OnScoreChanged?.Invoke(Score);
    }

    /// <summary>Finaliza el minijuego.</summary>
    private void EndGame()
    {
        IsPlaying = false;
        spawner?.ClearAllCircles();
        OnGameEnd?.Invoke(Score);
    }
}
