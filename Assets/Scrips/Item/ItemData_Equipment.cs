using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Equipment Data", menuName = "Item Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    [SerializeField] public EquipmentType equipmentType;
    [SerializeField] public StatsModifierData statsModifierData;
    [SerializeField] public List<ItemEffect> effects;
    [SerializeField] public float cooldown;

    public void AddModifiers()
    {
        PlayerManager.instance.player.cs.AddModifier(statsModifierData);
    }

    public void RemoveModifiers()
    {
        PlayerManager.instance.player.cs.RemoveModifier(statsModifierData);
    }

    public bool TryExcuteItemEffect(EffectExcuteData _target)
    {
        if(Inventory.instance.IsEquipmentCooldownFinish(equipmentType))
        {
            foreach (var effect in effects)
            {
                effect.ExcuteEffect(_target);
            }
            Inventory.instance.CooldownEquipment(equipmentType);
            return true;
        }
        return false;
    }

    public override string GetEffectText()
    {
        sb.Clear();
        if(cooldown > 0)
        {
            sb.Append("--");
            sb.Append("装备冷却时间：");
            sb.Append(cooldown.ToString());
            sb.Append("--");
            sb.AppendLine();
        }
        foreach (var effect in effects)
        {
            AddItemEffectText(effect);
        }
        return sb.ToString();
    }

    private void AddItemEffectText(ItemEffect _effect)
    {
        sb.Append(">>");
        sb.Append(_effect.GetDescription());
        sb.AppendLine();
    }
}
