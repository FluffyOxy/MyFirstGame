using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UI_TradeSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Color pressColor = Color.gray;
    private ItemData itemData;
    private int coinAmount;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (itemData is ItemData_Equipment)
        {
            UI.instance.tradeWindowMaterial.gameObject.SetActive(false);
            UI.instance.tradeWindowEquipment.Setup(itemData as ItemData_Equipment, coinAmount);
        }
        else
        {
            UI.instance.tradeWindowEquipment.gameObject.SetActive(false);
            UI.instance.tradeWindowMaterial.Setup(itemData, coinAmount);
        }
        GetComponent<Image>().color = pressColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<Image>().color = Color.white;
    }

    public void Setup(ItemData _item, int _coinAmount)
    {
        itemIcon.sprite = _item.icon;
        itemName.text = _item.itemName;
        itemData = _item;
        coinAmount = _coinAmount;
    }
}
