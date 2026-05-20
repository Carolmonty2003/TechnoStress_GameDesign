using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SortingItem
{
    public string itemLabel;

    [Header("Base Stat Changes")]
    public float stress;
    public float focus;
    public float anxiety;
    public float physicalHealth;
    public float academicProgress;
    public float digitalFatigue;
}

[CreateAssetMenu(fileName = "NewSortingEvent", menuName = "Semester Survivor/Sorting Event")]
public class SortingEventData : BaseEventData
{
    [Header("Items to Sort")]
    public List<SortingItem> items = new();

    [Header("Position Multipliers")]
    public List<float> positionMultipliers = new() { 1.0f, 0.5f, 0.2f };

    [Header("Feedback")]
    [TextArea(2, 4)]
    public string feedbackText;
}
