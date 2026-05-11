using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject that defines one in-game event.
/// Create one asset per event: right-click in Project > Create > Semester Survivor > Event
/// </summary>
[CreateAssetMenu(fileName = "NewEvent", menuName = "Semester Survivor/Event")]
public class EventData : ScriptableObject
{
    [Header("Event Content")]
    [Tooltip("Notification channel icon key, e.g. 'email', 'chat', 'campus'")]
    public string channel = "email";

    [Tooltip("Sender or source name shown above the message")]
    public string senderName;

    [TextArea(3, 6)]
    [Tooltip("The main event description the player reads")]
    public string eventText;

    [Header("Choices")]
    [Tooltip("Add 2+ choices. First is usually the 'good' option, last the 'bad' one — but order is up to you.")]
    public List<EventChoice> choices = new();
}
