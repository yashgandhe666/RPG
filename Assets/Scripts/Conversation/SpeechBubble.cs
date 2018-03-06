using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public string SpeechBubbleName;

    public Text GetTextField()
    {
        return GetComponentInChildren<Text>();
    }

    public Image GetImage()
    {
        return GetComponentInChildren<Image>();
    }
}
