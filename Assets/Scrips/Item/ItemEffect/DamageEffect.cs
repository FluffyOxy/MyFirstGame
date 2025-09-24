using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Effect", menuName = "Item Effect/Damage")]
public class DamageEffect : ItemEffect
{
    [Header("-1 ��ʾʹ�ý�ɫ����ֵ")]
    [SerializeField] protected DamageDataSerializable damageData;
    [SerializeField] protected bool isMagicEffectUseStatsValue = true;
    [SerializeField] protected bool isMagicBuff = false;

    public override void ExcuteEffect(EffectExcuteData _targetData)
    {
        DamageData data = CulculateDamageDamage();
        _targetData.target.GetStats().TakeDamage(data, PlayerManager.instance.player.transform);
    }

    protected DamageData CulculateDamageDamage()
    {
        CharacterStats stats = PlayerManager.instance.player.GetStats();

        DamageData data = damageData.GetDamageData();
        stats.CalculateDamageDataWithStats(ref data, isMagicEffectUseStatsValue, isMagicBuff);

        return data;
    }
}
