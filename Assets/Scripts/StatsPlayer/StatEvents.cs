using System;

public class StatEvents
{
    public static StatEvents Instance { get; } = new();

    public event Action<float> OnStressChanged;
    public event Action<float> OnFocusChanged;
    public event Action<float> OnAnxietyChanged;
    public event Action<float> OnPhysicalHealthChanged;
    public event Action<float> OnAcademicProgressChanged;
    public event Action<float> OnDigitalFatigueChanged;

    public event Action OnGameOver;
    public event Action OnDayEnded;

    public void StressChanged(float value)          => OnStressChanged?.Invoke(value);
    public void FocusChanged(float value)           => OnFocusChanged?.Invoke(value);
    public void AnxietyChanged(float value)         => OnAnxietyChanged?.Invoke(value);
    public void PhysicalHealthChanged(float value)  => OnPhysicalHealthChanged?.Invoke(value);
    public void AcademicProgressChanged(float value)=> OnAcademicProgressChanged?.Invoke(value);
    public void DigitalFatigueChanged(float value)  => OnDigitalFatigueChanged?.Invoke(value);

    public void GameOver()  => OnGameOver?.Invoke();
    public void DayEnded()  => OnDayEnded?.Invoke();
}
