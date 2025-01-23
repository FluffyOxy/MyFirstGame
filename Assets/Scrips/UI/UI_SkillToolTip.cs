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
    [SerializeField] TextMeshProUGUI priceText;

    public void Setup(string _text, string _name, Sprite _icon, float _price)
    {
        skillText.text = _text;
        nameText.text = _name;
        icon.sprite = _icon;
        priceText.text = _price.ToString();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
