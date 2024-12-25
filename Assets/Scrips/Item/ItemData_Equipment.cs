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

    public void AddModifiers()
    {
        PlayerManager.instance.player.cs.AddModifier(statsModifierData);
    }

    public void RemoveModifiers()
    {
        PlayerManager.instance.player.cs.RemoveModifier(statsModifierData);
    }

    public void ExcuteItemEffect(EffectExcuteData _target)
    {
        foreach (var effect in effects)
        {
            effect.ExcuteEffect(_target);
        }
    }

    public override string GetEffectText()
    {
        sb.Clear();
        foreach(var effect in effects)
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
