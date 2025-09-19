using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    public InventoryItem item = null;

    protected UI ui;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        if (item != null)
        {
            itemImage.color = Color.white;
            itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void Clear()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = null;
    }

    public bool IsNull()
    {
        return item == null;
    }

    public virtual void OnPointerDown(PointerEventData _eventData)
    {
        if(item == null)
        {
            return;
        }    
        if (_eventData.button == PointerEventData.InputButton.Left && canEquip())
        {
            Inventory.instance.EquipItem(item.data);
            ui.itemToolTip.HideToolTip();
            SceneAudioManager.instance.uiSFX.equip.Play(null);
        }
        else if(_eventData.button == PointerEventData.InputButton.Right)
        {
            Inventory.instance.DropItem(item.data);
            ui.itemToolTip.HideToolTip();
            SceneAudioManager.instance.uiSFX.discardInventory.Play(null);
        }
    }
    private bool canEquip()
    {
        return item.data.itemType == ItemType.Equipment;
    }

    public void OnPointerEnter(PointerEventData _eventData)
    {
        if (item == null || item.data == null)
        {
            return;
        }
        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
    }

    public void OnPointerExit(PointerEventData _eventData)
    {
        ui.itemToolTip.HideToolTip();
    }
}
