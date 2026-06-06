using UnityEngine;

/// <summary>
/// Genera círculos UI en posiciones aleatorias dentro de un panel RectTransform.
/// Los círculos son hijos del panel y se posicionan con anchoredPosition.
/// </summary>
public class CircleSpawner : MonoBehaviour
{
    [Header("Prefab UI")]
    [Tooltip("Prefab del círculo UI (debe tener RectTransform + Image + CircleTarget).")]
    public GameObject circlePrefab;

    [Header("Contenedor")]
    [Tooltip("Panel RectTransform donde se instancian los círculos.")]
    public RectTransform spawnArea;

    [Header("Configuración de Spawn")]
    [Tooltip("Tiempo en segundos entre cada aparición de círculo.")]
    public float spawnInterval = 0.8f;

    [Tooltip("Número máximo de círculos simultáneos en pantalla.")]
    public int maxCircles = 8;

    [Tooltip("Margen en píxeles desde los bordes del panel para evitar que los círculos se salgan.")]
    public float margin = 60f;

    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;

        // Si no se asignó spawnArea, usar este mismo RectTransform
        if (spawnArea == null)
            spawnArea = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (MinigameManager.Instance == null || !MinigameManager.Instance.IsPlaying)
            return;

        int currentCount = spawnArea != null ? spawnArea.childCount : 0;

        if (Time.time >= nextSpawnTime && currentCount < maxCircles)
        {
            SpawnCircle();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnCircle()
    {
        if (circlePrefab == null || spawnArea == null) return;

        // Obtener dimensiones del área de spawn
        Rect rect = spawnArea.rect;
        float halfW = rect.width  * 0.5f - margin;
        float halfH = rect.height * 0.5f - margin;

        // Protección: si el área es demasiado pequeña, usar márgenes cero
        if (halfW < 0) halfW = 0;
        if (halfH < 0) halfH = 0;

        float x = Random.Range(-halfW, halfW);
        float y = Random.Range(-halfH, halfH);

        // Instanciar como hijo del panel
        GameObject circle = Instantiate(circlePrefab, spawnArea);
        RectTransform rt  = circle.GetComponent<RectTransform>();

        if (rt != null)
        {
            rt.anchoredPosition = new Vector2(x, y);
        }
    }

    /// <summary>Destruye todos los círculos hijos del panel.</summary>
    public void ClearAllCircles()
    {
        if (spawnArea == null) return;

        // Recorrer al revés para destruir sin problemas de índice
        for (int i = spawnArea.childCount - 1; i >= 0; i--)
        {
            Destroy(spawnArea.GetChild(i).gameObject);
        }
    }
}
