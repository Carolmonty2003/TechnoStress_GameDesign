using System;
using UnityEngine;


[Serializable]
public class EventChoice
{
    [Header("Display")]
    [Tooltip("Text shown on the button")]
    public string buttonLabel;

    [Header("Stat Changes")]
    [Tooltip("Positive = increase, Negative = decrease")]
    public float stress;
    public float focus;
    public float anxiety;
    public float physicalHealth;
    public float academicProgress;
    public float digitalFatigue;

    [Header("Feedback")]
    [Tooltip("Short message shown after the player picks this choice")]
    [TextArea(2, 4)]
    public string feedbackText;

    [Header("Minigame")]
    [Tooltip("Si está activo, al elegir esta opción se lanza el minijuego de círculos.")]
    public bool launchMinigame;

    [Header("Time spent")]
    public float timeSpent;
}
