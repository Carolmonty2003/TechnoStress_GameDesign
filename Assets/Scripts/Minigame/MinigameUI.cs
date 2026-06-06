using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gestiona la interfaz de usuario del minijuego:
/// - Texto de tiempo restante
/// - Texto de puntuación
/// - Panel de fin de partida con puntuación final
/// - Botón de reinicio
/// </summary>
public class MinigameUI : MonoBehaviour
{
    [Header("HUD en Juego")]
    [Tooltip("Texto que muestra el tiempo restante.")]
    public TMP_Text timeText;

    [Tooltip("Texto que muestra la puntuación actual.")]
    public TMP_Text scoreText;

    [Header("Panel Fin de Partida")]
    [Tooltip("Panel que se muestra al terminar.")]
    public GameObject gameOverPanel;

    [Tooltip("Texto con la puntuación final dentro del panel.")]
    public TMP_Text finalScoreText;

    [Tooltip("Botón para reiniciar la partida.")]
    public Button restartButton;

    // ─────────────────────────────────────────────────────────────────────────
    void OnEnable()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Suscribirse a los eventos del manager
        if (MinigameManager.Instance != null)
        {
            MinigameManager.Instance.OnTimeChanged.AddListener(UpdateTime);
            MinigameManager.Instance.OnScoreChanged.AddListener(UpdateScore);
            MinigameManager.Instance.OnGameEnd.AddListener(ShowGameOver);
            MinigameManager.Instance.OnGameStart.AddListener(HideGameOver);
        }

        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);
    }

    void OnDisable()
    {
        // Limpiar listeners para evitar errores si el objeto se desactiva
        if (MinigameManager.Instance != null)
        {
            MinigameManager.Instance.OnTimeChanged.RemoveListener(UpdateTime);
            MinigameManager.Instance.OnScoreChanged.RemoveListener(UpdateScore);
            MinigameManager.Instance.OnGameEnd.RemoveListener(ShowGameOver);
            MinigameManager.Instance.OnGameStart.RemoveListener(HideGameOver);
        }
    }

    // ── Callbacks de eventos ─────────────────────────────────────────────────

    private void UpdateTime(float timeLeft)
    {
        if (timeText == null) return;
        int seconds = Mathf.CeilToInt(timeLeft);
        timeText.text = $"⏱ {seconds}s";

        // Avisar visualmente cuando queden menos de 5 segundos
        timeText.color = timeLeft <= 5f ? Color.red : Color.white;
    }

    private void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = $"⭐ {score}";
    }

    private void ShowGameOver(int finalScore)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = $"¡Puntuación final!\n{finalScore} pts";
    }

    private void HideGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    // ── Botón reinicio ───────────────────────────────────────────────────────

    private void OnRestartClicked()
    {
        MinigameManager.Instance?.StartGame();
    }
}
