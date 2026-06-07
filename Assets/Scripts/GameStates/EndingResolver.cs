using UnityEngine;

public enum EndingType
{
    ColapsaDigital,
    Equilibrada,
    Sobreviviste
}

public class EndingResolver : MonoBehaviour
{
    public static EndingType Resolve(bool finalDeliveryCompleted)
    {
        PlayerStats s = PlayerStats.Instance;

        float stress    = s.Stress.Value;
        float anxiety   = s.Anxiety.Value;
        float fatigue   = s.DigitalFatigue.Value;
        float academic  = s.AcademicProgress.Value;
        float focus     = s.Focus.Value;
        float health    = s.PhysicalHealth.Value;

        if (stress >= 80 && anxiety >= 80 && fatigue >= 80 && academic < 40 && !finalDeliveryCompleted)
            return EndingType.ColapsaDigital;

        if (stress <= 40 && anxiety <= 40 && fatigue <= 40 && academic >= 70 && focus >= 50 && health >= 40)
            return EndingType.Equilibrada;

        return EndingType.Sobreviviste;
    }
}
