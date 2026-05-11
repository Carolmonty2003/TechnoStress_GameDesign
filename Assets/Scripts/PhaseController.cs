using UnityEngine;
using System.Collections.Generic;
using Utils;

public class PhaseController : Singleton<PhaseController>
{
    [SerializeField] private List<float> phasesDurations;
    private List<float> currentPhaseDurations;
    private int currentPhaseIndex;

    void Start()
    {
        InitSingleton();
        currentPhaseDurations = phasesDurations;
        currentPhaseIndex = 0;
    }

    public bool SpendTime(float time)
    {
        if (currentPhaseDurations[currentPhaseIndex] < time) return false;
        currentPhaseDurations[currentPhaseIndex] -= time;
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
    }
}
