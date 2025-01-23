using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Sentence
{
    public string speakerName;
    public Sprite speakerIcon;
    public string content;
}

public class UI_CommunicationBlock : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI speakerName;
    [SerializeField] Image speakerIcon;
    [SerializeField] TextMeshProUGUI content;

    public void Setup(Sentence _sentence)
    {
        speakerName.text = _sentence.speakerName;
        speakerIcon.sprite = _sentence.speakerIcon;
        content.text = _sentence.content;
    }
}
