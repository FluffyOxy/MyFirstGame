using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillToolTip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI skillText;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI nameText;

    public void Setup(string _text, string _name, Sprite _icon)
    {
        skillText.text = _text;
        nameText.text = _name;
        icon.sprite = _icon;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
