using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats Increase Buff Effect", menuName = "Item Effect/Stats Increase Buff")]
public class StatsIncreaseBuff_Effect : ItemEffect
{
    [SerializeField] protected StatsModifierData modifierData;
    [SerializeField] protected float buffDuration;
    public override void ExcuteEffect(EffectExcuteData _target)
    {
        PlayerManager.instance.player.cs.StartCoroutine(PlayerManager.instance.player.cs.ModifyStatsInDurationCoroutine(modifierData, buffDuration));
    }
}
