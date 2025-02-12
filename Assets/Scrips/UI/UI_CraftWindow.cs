using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private UI_ItemToolTip ItemDetail;

    [SerializeField] private Image[] materialImages;

    private ItemData_Equipment equipment;

    public void Setup(ItemData_Equipment _equipment)
    {
        if (_equipment == null)
        {
            return;
        }

        gameObject.SetActive(true);

        equipment = _equipment;

        ItemDetail.ShowToolTip(_equipment);

        foreach(var material in materialImages)
        {
            TextMeshProUGUI amountText = material.GetComponentInChildren<TextMeshProUGUI>();
            amountText.color = Color.clear;
            material.color = Color.clear;
        }

        Assert.IsTrue(_equipment.craftingMaterials.Count <= materialImages.Length, "�����������͹����޷���ʾ");

        for(int i = 0; i < _equipment.craftingMaterials.Count; ++i)
        {
            materialImages[i].color = Color.white;
            materialImages[i].sprite = _equipment.craftingMaterials[i].data.icon;
            TextMeshProUGUI amountText = materialImages[i].GetComponentInChildren<TextMeshProUGUI>();
            amountText.color = Color.white;
            amountText.text = _equipment.craftingMaterials[i].stackSize.ToString();
        }
    }
    public void Craft()
    {
        SceneAudioManager.instance.uiSFX.craft.Play(null);

        if (equipment != null)
        {
            List<InventoryItem> lacks = Inventory.instance.TryGetMaterialsLackToCraft(equipment);
            if (lacks.Count == 0)
            {
                Inventory.instance.CraftEquipment_AfterCheck(equipment);
            }
            else
            {
                Debug.Log("Lack Materials");
                foreach (var lack in lacks)
                {
                    Debug.Log(lack.data.name + ": " + lack.stackSize);
                }
            }
        }
    }
}
