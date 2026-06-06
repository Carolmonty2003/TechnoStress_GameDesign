using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoodTips : MonoBehaviour
{
    public enum MoodType
    {
        NORMAL,
        TIRED,
        STRESSED,
        FATIGUED,
        FOCUSED,
        SATURATED
    }

    [Serializable]
    public struct Tip
    {
        public MoodType type;
        [TextArea(3, 10)] public string text;
    }

    [SerializeField] private List<Tip> tips;
    [SerializeField] private TMP_Text text;

    private Dictionary<MoodType, Tip> tipsDict = new Dictionary<MoodType, Tip>();

    void Start()
    {
        foreach(Tip tip in tips) 
            tipsDict.Add(tip.type, tip);

        LoadTip(MoodType.NORMAL);
    }

    public void LoadTip(MoodType mood) => text.text = tipsDict[mood].text;
}
