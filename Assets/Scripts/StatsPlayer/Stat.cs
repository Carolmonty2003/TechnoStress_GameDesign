using System;
using UnityEngine;

[Serializable]
public class Stat
{
    public float min;
    public float max;

    [SerializeField] private float _value;

    public float Value
    {
        get => _value;
        set => _value = Mathf.Clamp(value, min, max);
    }

    public float Normalized => Mathf.InverseLerp(min, max, _value);

    public Stat(float initial, float min = 0f, float max = 100f)
    {
        this.min = min;
        this.max = max;
        _value   = Mathf.Clamp(initial, min, max);
    }

    public void Add(float amount) => Value += amount;

    /* checks if value crossed a threshold going up or down,
       useful for triggering warnings only once */
    public bool CrossedAbove(float threshold, float previous) =>
        previous < threshold && _value >= threshold;

    public bool CrossedBelow(float threshold, float previous) =>
        previous > threshold && _value <= threshold;
}
