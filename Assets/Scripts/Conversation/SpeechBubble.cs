using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Knights.Enums;

public class SpeechBubble : MonoBehaviour
{
    public DialogueType speechType;

    public Text GetTextField()
    {
        return GetComponentInChildren<Text>();
    }

    public Image GetImage()
    {
        return GetComponentInChildren<Image>();
    }
}
