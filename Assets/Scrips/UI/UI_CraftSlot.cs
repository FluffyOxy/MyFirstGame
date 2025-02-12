using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CraftSlot : UI_ItemSlot, IPointerUpHandler
{
    [SerializeField] private Color pressColor;

    protected override void Start()
    {
        base.Start();
    }

    public void SetUpCraftSlot(ItemData_Equipment _data)
    {
        if (_data == null)
        {
            return;
        }

        item.data = _data;

        itemImage.sprite = _data.icon;
        itemText.text = _data.itemName;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        UI.instance.craftWindow.Setup(item.data as ItemData_Equipment);
        GetComponent<Image>().color = pressColor;
        SceneAudioManager.instance.uiSFX.buttonClick.Play(null);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<Image>().color = Color.white;
    }
}
