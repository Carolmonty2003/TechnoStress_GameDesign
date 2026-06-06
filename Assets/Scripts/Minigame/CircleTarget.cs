using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Círculo UI clickable. Al hacer clic suma puntos y se destruye.
/// No necesita Collider2D; usa el sistema de eventos de UI.
/// </summary>
public class CircleTarget : MonoBehaviour, IPointerClickHandler
{
    [Header("Configuración")]
    [Tooltip("Tiempo máximo (segundos) que el círculo permanece antes de desaparecer solo.")]
    public float lifetime = 3f;

    [Tooltip("Puntos que otorga al ser clicado.")]
    public int pointsValue = 10;

    private float spawnTime;

    void Start()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        // Desaparece solo si no lo clickan a tiempo
        if (Time.time - spawnTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>Callback del sistema de UI al hacer clic sobre este elemento.</summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (MinigameManager.Instance != null && MinigameManager.Instance.IsPlaying)
        {
            MinigameManager.Instance.AddScore(pointsValue);
            Destroy(gameObject);
        }
    }
}
