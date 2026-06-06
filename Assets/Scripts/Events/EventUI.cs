using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Controls the event panel, shows text and spawns choice buttons
public class EventUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject eventPanel;
    [SerializeField] private Image channelIcon;
    [SerializeField] private TMP_Text senderText;
    [SerializeField] private TMP_Text eventText;
    [SerializeField] private Transform choicesContainer;
    [SerializeField] private GameObject choiceButtonPrefab;

    [Header("Feedback")]
    [SerializeField] private GameObject feedbackPanel;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private Button continueButton;

    [Header("Channel Icons")]
    [SerializeField] private List<ChannelIconEntry> channelIcons = new();

    [Header("Penalty Banner (optional)")]
    [Tooltip("Panel que se muestra cuando el evento es un WorkIgnoredEventData. Puede ser null.")]
    [SerializeField] private GameObject penaltyBanner;
    [SerializeField] private TMP_Text penaltyBannerText;

    // fires when player presses continue after reading feedback
    public event Action OnEventResolved;

    private List<GameObject> spawnedButtons = new();

    private void Awake()
    {
        continueButton.onClick.AddListener(OnContinuePressed);
        feedbackPanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        eventPanel.SetActive(false);
    }

    private void Start()
    {
        // Al volver del minijuego: cerrar el panel del evento y avisar al EventManager
        // Se hace en Start() para garantizar que MinigameController.Instance ya está inicializado
        if (MinigameController.Instance != null)
            MinigameController.Instance.OnEventResolved += OnMinigameReturned;
    }

    private void OnMinigameReturned()
    {
        eventPanel.SetActive(false);
        OnEventResolved?.Invoke();
    }

    // call this to show an event to the player
    public void ShowEvent(EventData data)
    {
        // Muestra el banner de penalizacion si es un evento de trabajo ignorado
        //bool isPenalty = data is WorkIgnoredEventData;
        //if (penaltyBanner != null)
        //{
        //    penaltyBanner.SetActive(isPenalty);
        //    if (isPenalty && penaltyBannerText != null)
        //    {
        //        var wd = data as WorkIgnoredEventData;
        //        penaltyBannerText.text = $"⚠️ ¡PENALIZACIÓN! Ignoraste: {wd.ignoredTaskName}";
        //    }
        //}
        eventPanel.SetActive(true);
        feedbackPanel.SetActive(false);
        continueButton.gameObject.SetActive(false);

        senderText.text = data.senderName;
        eventText.text  = data.eventText;
        SetChannelIcon(data.channel);

        // destroy old buttons before spawning new ones
        foreach (var btn in spawnedButtons)
            Destroy(btn);
        spawnedButtons.Clear();

        // spawn one button per choice
        for (int i = 0; i < data.choices.Count; i++)
        {
            EventChoice choice = data.choices[i];
            GameObject btnGO = Instantiate(choiceButtonPrefab, choicesContainer);
            spawnedButtons.Add(btnGO);

            TMP_Text label = btnGO.GetComponentInChildren<TMP_Text>();
            if (label != null) label.text = choice.buttonLabel;

            // capture choice so the closure doesnt break
            Button btn = btnGO.GetComponent<Button>();
            EventChoice captured = choice;
            btn.onClick.AddListener(() => OnChoiceSelected(captured));
        }
    }

    // apply stats
    private void OnChoiceSelected(EventChoice choice)
    {
        PhaseController.Instance.SpendTime(choice.timeSpent);

        PlayerStats.Instance.ApplyChanges(
            stress:           choice.stress,
            focus:            choice.focus,
            anxiety:          choice.anxiety,
            physicalHealth:   choice.physicalHealth,
            academicProgress: choice.academicProgress,
            digitalFatigue:   choice.digitalFatigue
        );

        // disable all buttons so player cant pick two times in a row
        foreach (var btn in spawnedButtons)
            btn.GetComponent<Button>().interactable = false;

        // Lanzar el minijuego si la opción lo indica
        if (choice.launchMinigame)
        {
            eventPanel.SetActive(false); // ocultar el panel mientras dura el minijuego
            MinigameController.Instance?.LaunchMinigame();
            return; // el minijuego toma el control; OnMinigameReturned() retomará el flujo
        }

        if (!string.IsNullOrEmpty(choice.feedbackText))
        {
            feedbackPanel.SetActive(true);
            feedbackText.text = choice.feedbackText;
        }

        continueButton.gameObject.SetActive(true);
    }

    // hide the panel 
    private void OnContinuePressed()
    {
        eventPanel.SetActive(false);
        OnEventResolved?.Invoke();
    }

    // finds the right sprite for the channel or hides the icon
    private void SetChannelIcon(string channel)
    {
        if (channelIcon == null) return;
        var entry = channelIcons.Find(e => e.key == channel);
        if (entry != null && entry.sprite != null)
        {
            channelIcon.sprite  = entry.sprite;
            channelIcon.enabled = true;
        }
        else
        {
            channelIcon.enabled = false;
        }
    }

    [Serializable]
    public class ChannelIconEntry
    {
        public string key;
        public Sprite sprite;
    }
}
