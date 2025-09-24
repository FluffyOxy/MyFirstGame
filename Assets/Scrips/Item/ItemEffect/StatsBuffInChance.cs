using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats Increase Buff Effect", menuName = "Item Effect/Stats Increase Buff In Chance")]
public class StatsBuffInChance : StatsIncreaseBuff_Effect
{
    [Range(0, 1)][SerializeField] private float chance;
    public override void ExcuteEffect(EffectExcuteData _target)
    {
        if(Random.Range(0.0f, 1.0f) < chance)
        {
            base.ExcuteEffect(_target);
        }
    }
}
