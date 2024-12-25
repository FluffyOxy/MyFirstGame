using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Vampire Strike Effect", menuName = "Item Effect/Vampire Strike")]
public class VampireStrike_Effect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercentage;
    public override void ExcuteEffect(EffectExcuteData _data)
    {
        PlayerManager.instance.player.cs.Heal(healPercentage * _data.damage);
    }
}
