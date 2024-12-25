using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatsDescriptionToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statDescriptionText;

    public void ShowToolTip(StatType _type, string _description)
    {
        statNameText.text = Translater.instance.Translate(_type.ToString());
        statDescriptionText.text = _description;

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
