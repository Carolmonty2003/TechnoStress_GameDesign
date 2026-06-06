using TMPro;
using UnityEngine;

public class EventsOfTypeUI : MonoBehaviour
{
    [SerializeField] private string channel;
    private int count = 0;
    [SerializeField] private TMP_Text text;
    [SerializeField] private EventUI eventUI;

    void Start()
    {
        LoadEventCount();
        eventUI.OnEventResolved += LoadEventCount;
    }

    public void LoadEventCount()
    {
        count = 0;
        foreach(BaseEventData e in EventManager.Instance.EventsLeft)
            if(e.channel == channel)
                count++;

        text.text = count.ToString();
    }
}
