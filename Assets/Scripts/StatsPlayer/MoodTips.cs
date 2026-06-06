using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        public Sprite image;
    }

    [SerializeField] private List<Tip> tips;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image tipImage;

    private Dictionary<MoodType, Tip> tipsDict = new Dictionary<MoodType, Tip>();

    void Start()
    {
        foreach(Tip tip in tips) 
            tipsDict.Add(tip.type, tip);

        LoadTip(MoodType.NORMAL);
    }

    public void LoadTip(MoodType mood)
    {
        Tip tip = tipsDict[mood];
        text.text = tip.text;
        if (tipImage != null)
        {
            tipImage.sprite  = tip.image;
            tipImage.enabled = tip.image != null;
        }
    }
}

