using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Item Effect/Heal")]
public class HealInPercentage_Effect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float percentage;
    public override void ExcuteEffect(EffectExcuteData _target)
    {
        Player player = PlayerManager.instance.player;
        player.cs.Heal(percentage * player.cs.GetMaxHealthValue());
        SceneAudioManager.instance.itemSFX.heal.Play(null);
    }
}
