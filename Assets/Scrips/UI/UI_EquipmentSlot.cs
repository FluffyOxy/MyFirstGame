using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType equipmentType;

    public override void OnPointerDown(PointerEventData _eventData)
    {
        if(item == null || item.data == null)
        {
            return;
        }
        Inventory.instance.UnequipItem(item.data);
        UI.instance.itemToolTip.HideToolTip();
    }

    private void OnValidate()
    {
        gameObject.name = "Equipment Slot = " + equipmentType.ToString();
    }

    
}
