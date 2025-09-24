using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ModifyPostDamageEffectData Effect", menuName = "Item Effect/Modify Post Damage Effect Data")]
public class ModifyPostDamageEffectData : ItemEffect
{
    [SerializeField] private PostDamageEffectData postDamageEffectData;
    [SerializeField] private float duration;

    public override void ExcuteEffect(EffectExcuteData _targetData)
    {
        (PlayerManager.instance.player.GetStats() as PlayerStats).ModifyPostDamageEffectDataInTime(postDamageEffectData, duration);
    }
}
