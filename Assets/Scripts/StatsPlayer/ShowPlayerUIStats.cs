using UnityEngine;
using TMPro;

public class ShowPlayerUIStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI anxiety;
    [SerializeField] private TextMeshProUGUI stress;
    [SerializeField] private TextMeshProUGUI focus;
    [SerializeField] private TextMeshProUGUI fisical;
    [SerializeField] private TextMeshProUGUI academic;
    [SerializeField] private TextMeshProUGUI digital;
  
    void Update()
    {
        anxiety.text =  "Anxiety :" + PlayerStats.Instance.Anxiety.Value.ToString();
        stress.text = "Stress :" + PlayerStats.Instance.Stress.Value.ToString();
        focus.text = "Focus :" + PlayerStats.Instance.Focus.Value.ToString();
        fisical.text = "Physical :" + PlayerStats.Instance.PhysicalHealth.Value.ToString();
        academic.text = "Academic :" + PlayerStats.Instance.AcademicProgress.Value.ToString();
        digital.text = "Digital :" + PlayerStats.Instance.DigitalFatigue.Value.ToString();
    }
}
