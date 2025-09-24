using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SelfDamage Effect", menuName = "Item Effect/SelfDamage")]
public class SelfDamage : DamageEffect
{
    public override void ExcuteEffect(EffectExcuteData _target)
    {
        DamageData data = CulculateDamageDamage();
        Player player = PlayerManager.instance.player;
        player.GetStats().TakeDamage(data, player.transform);
    }
}
