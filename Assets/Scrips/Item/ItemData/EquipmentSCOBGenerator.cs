using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New EquipmentSCOBGenerator", menuName = "ExcelData/Equipment")]
public class EquipmentSCOBGenerator : ScriptableObject
{
    [Header("填写下方二者后右键上方标题导入数据")]
    [SerializeField] private string targetFile;
    [SerializeField] private EquipmentData dataSet;

    [ContextMenu("导入数据")]
    public void ImportData()
    {
#if UNITY_EDITOR
        if(dataSet == null)
        {
            Debug.LogError("未提供数据来源");
            return;
        }

        foreach(ExcelEquipmentData data in dataSet.NewEquipment)
        {
            ItemData_Equipment newEquipment = AssetDatabase.LoadAssetAtPath<ItemData_Equipment>(targetFile + "/" + data.ObjectName + ".asset");
            if(newEquipment == null)
            {
                newEquipment = ScriptableObject.CreateInstance<ItemData_Equipment>();
                LoadData(newEquipment, data);
                UnityEditor.AssetDatabase.CreateAsset(newEquipment, targetFile + "/" + data.ObjectName + ".asset");
            }
            else
            {
                LoadData(newEquipment, data);
            }
        }
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }

    private void LoadData(ItemData_Equipment _newEquipment, ExcelEquipmentData _data)
    {
        _newEquipment.itemName = _data.ItemName;
        _newEquipment.itemType = ItemType.Equipment;
        _newEquipment.equipmentType = TransTypeFromString(_data.EquipmentType);

        _newEquipment.description = _data.Description;
        _newEquipment.detail = _data.Detail;
        _newEquipment.price = (int)_data.Price;
        _newEquipment.cooldown = _data.CoolDown;

        _newEquipment.statsModifierData = new StatsModifierData();
        _newEquipment.statsModifierData.strength = _data.Strength;
        _newEquipment.statsModifierData.agility = _data.Agility;
        _newEquipment.statsModifierData.intelligence = _data.Intelligence;
        _newEquipment.statsModifierData.vitality = _data.Vitality;

        _newEquipment.statsModifierData.maxHealth = _data.MaxHealth;
        _newEquipment.statsModifierData.armor = _data.Armor;
        _newEquipment.statsModifierData.evasion = _data.Evasion;
        _newEquipment.statsModifierData.magicResistance = _data.MagicResistance;
        _newEquipment.statsModifierData.maxFlaskUsageTime = _data.MaxFlaskUsageTime;
        _newEquipment.statsModifierData.flaskUsageRecover = _data.FlaskUsageRecover;

        _newEquipment.statsModifierData.damage = _data.Damage;
        _newEquipment.statsModifierData.critChance = _data.CritChance;
        _newEquipment.statsModifierData.critPower = _data.CritPower;
        _newEquipment.statsModifierData.attackSpeed = _data.AttackSpeed;

        _newEquipment.statsModifierData.fireDamage = _data.FireDamage;
        _newEquipment.statsModifierData.iceDamage = _data.IceDamage;
        _newEquipment.statsModifierData.lightningDamage = _data.LightningDamage;
        _newEquipment.statsModifierData.fireDuration = _data.FireDuration;
        _newEquipment.statsModifierData.iceDuration = _data.IceDuration;
        _newEquipment.statsModifierData.lightningDuration = _data.LightningDuration;

        _newEquipment.statsModifierData.fireDamageCooldown = _data.FireDamageCooldown;
        _newEquipment.statsModifierData.fireDamageTransform = _data.FireDamageTransform;
        _newEquipment.statsModifierData.chillSlowRate = _data.ChillSlowRate;
        _newEquipment.statsModifierData.chillArmorReduce = _data.ChillArmorReduce;
        _newEquipment.statsModifierData.shockAccuracyReduce = _data.ShockAccuracyReduce;
        _newEquipment.statsModifierData.thunderStrikeCount = _data.ThunderStrikeCount;
        _newEquipment.statsModifierData.thunderStrikeRate = _data.ThunderStrikeRate;
    }

    private EquipmentType TransTypeFromString(string _type)
    {
        switch(_type)
        {
            case "Amulet": return EquipmentType.Amulet;
            case "Weapon": return EquipmentType.Weapon;
            case "Flask": return EquipmentType.Flask;
            case "Armor": return EquipmentType.Armor;
            default: Debug.LogError("Undefine equipment type: " + _type); return EquipmentType.Weapon;
        }
    }
}
