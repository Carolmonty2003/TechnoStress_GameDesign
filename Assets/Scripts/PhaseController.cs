using UnityEngine;
using System.Collections.Generic;
using Utils;

public class PhaseController : Singleton<PhaseController>
{
    [SerializeField] private List<float> phasesDurations;
    private List<float> currentPhaseDurations;
    private int currentPhaseIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPhaseDurations = phasesDurations;
        currentPhaseIndex = 0;
    }

    public bool SpendTime(float time)
    {
        if (currentPhaseDurations[currentPhaseIndex] < time) return false;
        currentPhaseDurations[currentPhaseIndex] -= time;
        return true;
    }

    public void PassPhase()
    {
        currentPhaseIndex++;
        if(currentPhaseIndex == phasesDurations.Count)
        {
            currentPhaseIndex = 0;
            currentPhaseDurations = phasesDurations;
        }
    }
}
