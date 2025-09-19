using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;
using System.Reflection;

public class UI_ItemToolTip : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemEffectText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private Image image;

    [Header("Stats UI")]
    [SerializeField] private Transform statsSlotParent;
    [SerializeField] private UI_StatsSlot[] statSlot;

    public void ShowToolTip(ItemData_Equipment _equipment)
    {
        if (_equipment != null)
        {
            itemNameText.text = Translator.instance.Translate(_equipment.itemName);
            itemTypeText.text = Translator.instance.Translate(_equipment.equipmentType.ToString());
            image.sprite = _equipment.icon;

            for(int i = 0; i < statSlot.Length; i++)
            {
                statSlot[i].Activate();
                statSlot[i].UpdateStatsSlotUI_Modifier(_equipment.statsModifierData);
                if(!statSlot[i].isChange)
                {
                    statSlot[i].Hide();
                }
            }

            itemEffectText.text = _equipment.GetEffectText();
            itemDescriptionText.text = _equipment.description;
        }
        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
