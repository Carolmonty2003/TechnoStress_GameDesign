using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEvent", menuName = "Semester Survivor/Event")]
public class EventData : BaseEventData
{
    [Header("Choices")]
    public List<EventChoice> choices = new();
}
