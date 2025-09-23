using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StunInChance Effect", menuName = "Item Effect/StunInChance")]
public class StunInChanceEffect : ItemEffect
{
    [Range(0, 1)][SerializeField] private float chance;
    public override void ExcuteEffect(EffectExcuteData _targetData)
    {
        if((Random.Range(0, 100) < chance * 100) && (_targetData.target is Enemy))
        {
            Enemy stunEnemy = _targetData.target as Enemy;
            stunEnemy.OpenCounterAttackWindow();
            stunEnemy.TryToBeStuuned();
            stunEnemy.CloseCounterAttackWindow();
        }
    }
}
