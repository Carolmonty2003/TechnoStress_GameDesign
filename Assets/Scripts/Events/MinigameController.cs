using System;
using UnityEngine;
using Utils;

/// <summary>
/// Orquesta la transición entre la UI normal del juego y el minijuego de círculos.
/// Oculta el Canvas de la app, muestra el Canvas del minijuego, lanza la partida,
/// y al terminar restaura el estado anterior.
/// </summary>
public class MinigameController : Singleton<MinigameController>
{
    [Header("Canvas")]
    [Tooltip("Canvas principal de la aplicación (se oculta durante el minijuego).")]
    [SerializeField] private GameObject appCanvas;

    [Tooltip("Canvas del minijuego de círculos (se muestra durante el minijuego).")]
    [SerializeField] private GameObject minigameCanvas;

    /// <summary>Se dispara cuando el minijuego termina, para que EventManager avance al siguiente evento.</summary>
    public event Action OnEventResolved;

    // ─────────────────────────────────────────────────────────────────────────
    void Awake()
    {
        InitSingleton();
    }

    void Start()
    {
        // El Canvas del minijuego empieza oculto
        if (minigameCanvas != null)
            minigameCanvas.SetActive(false);

        // Suscribirse al final del minijuego
        if (MinigameManager.Instance != null)
            MinigameManager.Instance.OnGameEnd.AddListener(OnMinigameFinished);
    }

    // ── API pública ──────────────────────────────────────────────────────────

    /// <summary>
    /// Oculta la UI de la app y arranca el minijuego.
    /// Puede llamarse desde el onClick de cualquier botón instanciado por código.
    /// </summary>
    public void LaunchMinigame()
    {
        if (appCanvas != null)      appCanvas.SetActive(false);
        if (minigameCanvas != null)  minigameCanvas.SetActive(true);

        MinigameManager.Instance?.StartGame();
    }

    // ── Callbacks ────────────────────────────────────────────────────────────

    private void OnMinigameFinished(int score)
    {
        if (minigameCanvas != null)  minigameCanvas.SetActive(false);
        if (appCanvas != null)       appCanvas.SetActive(true);

        OnEventResolved?.Invoke();
    }
}
