using UnityEngine;

/// <summary>
/// Fuerza la resolución y modo ventana al arrancar la build.
/// Añadir este script a un GameObject en la primera escena.
/// </summary>
public class ForceResolution : MonoBehaviour
{
    private const int TargetWidth  = 941;
    private const int TargetHeight = 1672;

    private void Awake()
    {
#if !UNITY_EDITOR
        Screen.SetResolution(TargetWidth, TargetHeight, FullScreenMode.Windowed);
#endif
    }
}
