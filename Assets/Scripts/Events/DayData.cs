using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Day", menuName = "Semester Survivor/Day")]
public class DayData : ScriptableObject
{
    public List<BaseEventData> events = new();
}
