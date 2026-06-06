using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventList : MonoBehaviour
{
    [SerializeField] private TMP_Text[] texts;
    [SerializeField] private EventUI eventUI;

    void Start()
    {
        LoadEventList();
        eventUI.OnEventResolved += LoadEventList;
    }

    public void LoadEventList()
    {
        List<BaseEventData> events = EventManager.Instance.EventsLeft;

        for (int i = 0; i < texts.Length; i++) texts[i].text = "";

        for (int i = 0; i < Mathf.Min(texts.Length, events.Count); i++)
            texts[i].text = events[i].Name;
            
    }
}
