using UnityEngine;
using System.Collections.Generic;
using Utils;
using TMPro;
using System;

public class PhaseController : Singleton<PhaseController>
{
    [SerializeField] private List<float> phasesDurations;
    private List<float> currentPhaseDurations;
    private int currentPhaseIndex;
    [SerializeField] private float startTotalTime = 8 * 60;
    private float totalTime;

    private bool allEventsDone = false;

    [SerializeField] private TMP_Text hourText;

    public Action OnTimeSpent;
    public Action OnDayEnded;
    public int CurrentHour => Mathf.FloorToInt(totalTime / 60f);

    void Awake()
    {
        InitSingleton();
        totalTime = startTotalTime;
        currentPhaseDurations = phasesDurations;
        currentPhaseIndex = 0;
        hourText.text = MakeHour() + ":" + MakeMinutes();
    }

    private string MakeHour()
    {
        int h = Mathf.FloorToInt(totalTime / 60f) % 24;
        return h.ToString();
    }

    private string MakeMinutes()
    {
        int m = Mathf.FloorToInt(totalTime % 60);
        return m < 10 ? "0" + m : m.ToString();
    }

    public bool SpendTime(float time)
    {
        if (currentPhaseDurations.Count > currentPhaseIndex)
            currentPhaseDurations[currentPhaseIndex] -= time;

        totalTime += time + ((PlayerStats.Instance.FatiguePunishment) ? 30 : 0);
        if (totalTime > 24 * 60) PlayerStats.Instance.ApplyChanges(digitalFatigue: 20);
        else if (totalTime > 23 * 60) PlayerStats.Instance.ApplyChanges(digitalFatigue: 10);

        hourText.text = MakeHour() + ":" + MakeMinutes();
        OnTimeSpent?.Invoke();

        if(PlayerStats.Instance.DigitalFatigue.Value >= 80 && totalTime >= 23 * 60)
        {
            PlayerStats.Instance.FatiguePunishment = true;
            Restart();
        }

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
        totalTime = startTotalTime;
        allEventsDone = false;
        hourText.text = MakeHour() + ":" + MakeMinutes();
    }

    public void Rest()
    {
        if (totalTime < 24 * 60 && !allEventsDone)
        {
            SpendTime(60);
            PlayerStats.Instance.ApplyChanges(digitalFatigue: -10, stress: -10, anxiety: -10);
            return;
        }

        PlayerStats.Instance.ApplyChanges(digitalFatigue: -20, stress: -20, anxiety: -20);
        EndDay();
    }

    public void EndDay()
    {
        allEventsDone = false;
        Restart();
        OnDayEnded?.Invoke();
    }

    public void AllEventsDone() => allEventsDone = true; 
}
