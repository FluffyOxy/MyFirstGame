using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatsSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private TextMeshProUGUI statNameText;
    [Space]
    [SerializeField] private StatType type;

    [TextArea]
    [SerializeField] private string statDescription;

    private UI ui;

    private string statName;
    public bool isChange { get; private set; } = false;

    private void OnValidate()
    {
        statName = type.ToString();
        gameObject.name = "Stat = " + statName;
    }

    private void Start()
    {
        statName = Translator.instance.Translate(statName);
        statNameText.text = statName + ":";
        ui = GetComponentInParent<UI>();
    }

    public void UpdateStatsSlotUI()
    {
        float value = PlayerManager.instance.player.cs.GetStatByType(type);
        statText.text = value.ToString();
    }

    public void UpdateStatsSlotUI_Modifier(StatsModifierData _modifier)
    {
        float value = _modifier.GetStatByType(type);
        if (value > 0)
        {
            statText.text = "+" + value.ToString();
        }
        else
        {
            statText.text = value.ToString();
        }
        if (_modifier.GetStatByType(type) != 0)
        {
            isChange = true;
        }
        else
        {
            isChange = false;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData _eventData)
    {
        ui.statsDescriptionToolTip.ShowToolTip(type, statDescription);
    }

    public void OnPointerExit(PointerEventData _eventData)
    {
        ui.statsDescriptionToolTip.HideToolTip();
    }
}