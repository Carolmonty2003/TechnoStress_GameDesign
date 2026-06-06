using UnityEngine;

public abstract class BaseEventData : ScriptableObject
{
    public string Name;

    [Header("Schedule")]
    [Tooltip("Hora del dia en la que aparece esta tarea en la lista (ej: 8 = 8:00)")]
    public int scheduledHour = 8;

    [Header("Event Content")]
    public string channel = "email";
    public string senderName;

    [TextArea(3, 6)]
    public string eventText;

    [Header("Effects if Ignored")]
    public float stress = 0;
    public float focus = 0;
    public float anxiety = 0;
    public float physicalHealth = 0;
    public float academicProgress = 0;
    public float digitalFatigue = 0;
}
