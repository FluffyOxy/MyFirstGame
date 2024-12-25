using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_WarningToolTip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public void ShowWarning(string _text)
    {
        gameObject.SetActive(true);
        text.text = _text;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
