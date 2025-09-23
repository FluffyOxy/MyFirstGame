using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Lightning Effect", menuName = "Item Effect/Damage")]
public class DamageEffect : ItemEffect
{
    [Header("-1 表示使用角色属性值")]
    [SerializeField] private DamageDataSerializable damageData;
    [SerializeField] private bool isMagicEffectUseStatsValue = true;

    public override void ExcuteEffect(EffectExcuteData _target)
    {
        CharacterStats stats = PlayerManager.instance.player.GetStats();

        DamageData data = damageData.GetDamageData();
        stats.CalculateDamageDataWithStats(ref data, isMagicEffectUseStatsValue, false);

        _target.target.GetStats().TakeDamage(data, PlayerManager.instance.player.transform);
    }
}
