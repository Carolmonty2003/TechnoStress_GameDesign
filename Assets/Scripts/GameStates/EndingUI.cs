using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct EndingDisplayData
{
    public Sprite imageSprite;
    public string titleText;
    [TextArea(3, 10)] public string descriptionText;
}

public class EndingUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image endingImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    [Header("Endings Data")]
    [SerializeField] private EndingDisplayData colapsoDigitalData;
    [SerializeField] private EndingDisplayData equilibradaData;
    [SerializeField] private EndingDisplayData sobrevivisteData;

    [Header("Final Delivery (Temporal)")]
    [SerializeField] private bool finalDeliveryCompleted = false;

    private void Start()
    {
        EndingType ending = EndingResolver.Resolve(finalDeliveryCompleted);
        ShowEnding(ending);
    }

    private void ShowEnding(EndingType ending)
    {
        EndingDisplayData dataToApply;

        switch (ending)
        {
            case EndingType.ColapsaDigital:
                dataToApply = colapsoDigitalData;
                break;
            case EndingType.Equilibrada:
                dataToApply = equilibradaData;
                break;
            case EndingType.Sobreviviste:
            default:
                dataToApply = sobrevivisteData;
                break;
        }

        if (endingImage != null)
        {
            endingImage.sprite = dataToApply.imageSprite;
            endingImage.enabled = dataToApply.imageSprite != null;
        }
        
        if (titleText != null) titleText.text = dataToApply.titleText;
        if (descriptionText != null) descriptionText.text = dataToApply.descriptionText;
    }
}
