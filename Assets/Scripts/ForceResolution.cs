using UnityEngine;

/// <summary>
/// Ajusta la ventana de la build al mayor tamaño posible con la proporción
/// 941:1672 que quepa en el monitor. Mantiene el aspect ratio para que se vea
/// igual que en el Game view del editor, sin deformarse.
/// Se ejecuta automáticamente antes de cargar la primera escena; no necesita
/// estar en ningún GameObject.
/// </summary>
public static class ForceResolution
{
    private const int   DesignWidth  = 941;
    private const int   DesignHeight = 1672;
    private const float TargetAspect = (float)DesignWidth / DesignHeight; // ancho / alto

    // BeforeSceneLoad garantiza que se ejecuta antes que cualquier Awake() de la escena,
    // incluido el CanvasScaler. Así la resolución ya está fijada cuando el Canvas calcula su escala.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
#if !UNITY_EDITOR
        // Resolución nativa del monitor principal
        int monitorW = Display.main.systemWidth;
        int monitorH = Display.main.systemHeight;

        // Margen para la barra de título de la ventana y la barra de tareas de Windows
        int maxW = monitorW;
        int maxH = Mathf.RoundToInt(monitorH * 0.90f);

        // Mayor tamaño con proporción 941:1672 que quepa en pantalla
        int height = Mathf.Min(DesignHeight, maxH);
        int width  = Mathf.RoundToInt(height * TargetAspect);

        if (width > maxW)
        {
            width  = maxW;
            height = Mathf.RoundToInt(width / TargetAspect);
        }

        Screen.SetResolution(width, height, FullScreenMode.Windowed);
#endif
    }
}
