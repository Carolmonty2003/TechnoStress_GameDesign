using UnityEngine;

/// <summary>
/// Fuerza la resolución ANTES de que cualquier escena cargue sus Awake/Start.
/// No necesita estar en ningún GameObject — se ejecuta automáticamente.
/// </summary>
public static class ForceResolution
{
    private const int TargetWidth  = 941;
    private const int TargetHeight = 1672;

    // BeforeSceneLoad garantiza que se ejecuta antes que cualquier Awake() de la escena,
    // incluido el CanvasScaler. Así la resolución ya está fijada cuando el Canvas calcula su escala.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
#if !UNITY_EDITOR
        Screen.SetResolution(TargetWidth, TargetHeight, FullScreenMode.Windowed);
#endif
    }
}
