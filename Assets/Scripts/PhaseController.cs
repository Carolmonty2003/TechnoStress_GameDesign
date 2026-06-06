using UnityEngine;
using System.Collections.Generic;
using Utils;
using TMPro;

public class PhaseController : Singleton<PhaseController>
{
    [SerializeField] private List<float> phasesDurations;
    private List<float> currentPhaseDurations;
    private int currentPhaseIndex;
    private float totalTime = 600.0f;

    [SerializeField] private TMP_Text hourText;

    void Awake()
    {
        InitSingleton();
        currentPhaseDurations = phasesDurations;
        currentPhaseIndex = 0;
        hourText.text = "10:00";
    }

    public bool SpendTime(float time)
    {
        if (currentPhaseDurations[currentPhaseIndex] < time) return false;
        currentPhaseDurations[currentPhaseIndex] -= time;
        totalTime += time;
        if (totalTime > 24 * 60) PlayerStats.Instance.ApplyChanges(digitalFatigue: 20);
        else if (totalTime > 23 * 60) PlayerStats.Instance.ApplyChanges(digitalFatigue: 10);
        hourText.text = Mathf.FloorToInt(totalTime / 60.0f).ToString() + ":00";
        return true;
    }

    public bool PassPhase()
    {
        currentPhaseIndex++;
        return currentPhaseIndex == phasesDurations.Count;
    }

    public void Restart()
    {
        currentPhaseDurations = phasesDurations;
        currentPhaseIndex = 0;
        totalTime = 0.0f;
    }
}
