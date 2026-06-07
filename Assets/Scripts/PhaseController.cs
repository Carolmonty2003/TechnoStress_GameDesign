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
    public int CurrentHour => Mathf.FloorToInt(totalTime / 60f);

    void Awake()
    {
        InitSingleton();
        totalTime = startTotalTime;
        currentPhaseDurations = phasesDurations;
        currentPhaseIndex = 0;
        hourText.text = Mathf.FloorToInt(totalTime / 60.0f).ToString() + ":" + MakeMinutes();
    }

    private string MakeMinutes()
    {
        if(totalTime % 60 < 10) return "0" + (totalTime % 60).ToString();
        else return (totalTime % 60).ToString();
    }

    public bool SpendTime(float time)
    {
        if (currentPhaseDurations.Count > currentPhaseIndex)
            currentPhaseDurations[currentPhaseIndex] -= time;

        totalTime += time + ((PlayerStats.Instance.FatiguePunishment) ? 30 : 0);
        if (totalTime > 24 * 60) PlayerStats.Instance.ApplyChanges(digitalFatigue: 20);
        else if (totalTime > 23 * 60) PlayerStats.Instance.ApplyChanges(digitalFatigue: 10);

        hourText.text = Mathf.FloorToInt(totalTime / 60.0f).ToString() + ":" + MakeMinutes();
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
        hourText.text = Mathf.FloorToInt(totalTime / 60.0f).ToString() + ":" + MakeMinutes();
    }

    public void Rest()
    {
        if (totalTime < 24 * 60 && !allEventsDone)
        {
            SpendTime(60); //Spend 1h
            PlayerStats.Instance.ApplyChanges(digitalFatigue: -10, stress: -10, anxiety: -10);
            return;
        }

        PlayerStats.Instance.ApplyChanges(digitalFatigue: -20, stress: -20, anxiety: -20);
        Restart();
    }

    public void AllEventsDone() => allEventsDone = true; 
}
