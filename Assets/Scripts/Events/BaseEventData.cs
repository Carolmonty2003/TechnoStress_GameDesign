using UnityEngine;

public abstract class BaseEventData : ScriptableObject
{
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
