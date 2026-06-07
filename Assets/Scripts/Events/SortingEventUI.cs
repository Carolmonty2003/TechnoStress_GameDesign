using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SortingEventUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image channelIcon;
    [SerializeField] private TMP_Text senderText;
    [SerializeField] private TMP_Text eventText;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private GameObject sortingItemPrefab;
    [SerializeField] private Button submitButton;

    [Header("Feedback")]
    [SerializeField] private GameObject feedbackPanel;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private Button continueButton;

    [Header("Channel Icons")]
    [SerializeField] private List<ChannelIconEntry> channelIcons = new();

    public event Action OnEventResolved;

    private List<GameObject> spawnedItems = new();
    private SortingEventData currentEventData;

    private void Awake()
    {
        submitButton.onClick.AddListener(OnSubmitPressed);
        continueButton.onClick.AddListener(OnContinuePressed);
        
        feedbackPanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ShowEvent(SortingEventData data)
    {
        currentEventData = data;
        gameObject.SetActive(true);
        feedbackPanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(true);

        senderText.text = data.senderName;
        eventText.text = data.eventText;
        SetChannelIcon(data.channel);

        foreach (var item in spawnedItems)
        {
            Destroy(item);
        }
        spawnedItems.Clear();

        List<SortingItem> randomizedList = new(data.items);
        for (int i = randomizedList.Count - 1; i > 0; i--)
        {
            int r = UnityEngine.Random.Range(0, i + 1);
            SortingItem temp = randomizedList[i];
            randomizedList[i] = randomizedList[r];
            randomizedList[r] = temp;
        }

        foreach (var itemData in randomizedList)
        {
            GameObject itemGO = Instantiate(sortingItemPrefab, itemsContainer);
            spawnedItems.Add(itemGO);

            SortingUIItem uiItem = itemGO.GetComponent<SortingUIItem>();
            if (uiItem != null)
            {
                uiItem.Setup(itemData);
            }
        }
    }

    private void OnSubmitPressed()
    {
        if (currentEventData == null) return;

        List<SortingUIItem> orderedItems = new();
        foreach (Transform child in itemsContainer)
        {
            SortingUIItem uiItem = child.GetComponent<SortingUIItem>();
            if (uiItem != null)
            {
                orderedItems.Add(uiItem);
            }
        }

        float totalStress = 0;
        float totalFocus = 0;
        float totalAnxiety = 0;
        float totalPhysicalHealth = 0;
        float totalAcademicProgress = 0;
        float totalDigitalFatigue = 0;

        for (int i = 0; i < orderedItems.Count; i++)
        {
            SortingItem item = orderedItems[i].ItemData;
            
            float multiplier = 0f;
            if (currentEventData.positionMultipliers.Count > 0)
            {
                int multIndex = Mathf.Min(i, currentEventData.positionMultipliers.Count - 1);
                multiplier = currentEventData.positionMultipliers[multIndex];
            }

            totalStress += item.stress * multiplier;
            totalFocus += item.focus * multiplier;
            totalAnxiety += item.anxiety * multiplier;
            totalPhysicalHealth += item.physicalHealth * multiplier;
            totalAcademicProgress += item.academicProgress * multiplier;
            totalDigitalFatigue += item.digitalFatigue * multiplier;
        }

        PlayerStats.Instance.ApplyChanges(
            stress: totalStress,
            focus: totalFocus,
            anxiety: totalAnxiety,
            physicalHealth: totalPhysicalHealth,
            academicProgress: totalAcademicProgress,
            digitalFatigue: totalDigitalFatigue
        );


        submitButton.gameObject.SetActive(false);
        foreach (var itemGO in spawnedItems)
        {
            var grp = itemGO.GetComponent<CanvasGroup>();
            if (grp != null)
            {
                grp.blocksRaycasts = false;
            }
        }

        if (!string.IsNullOrEmpty(currentEventData.feedbackText))
        {
            feedbackPanel.SetActive(true);
            feedbackText.text = currentEventData.feedbackText;
        }

        continueButton.gameObject.SetActive(true);
    }

    private void OnContinuePressed()
    {
        gameObject.SetActive(false);
        OnEventResolved?.Invoke();
    }

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
