using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike_EffectController : MonoBehaviour
{
    Entity target;
    DamageData damage;

    public void SetUp(Entity _target, DamageData _damage)
    {
        target = _target;
        damage = _damage;
        SceneAudioManager.instance.itemSFX.thunderAttack.Play(null);
    }

    public void AnimFinishTrigger()
    {
        Destroy(gameObject);
    }

    public void AnimDamageTrigger()
    {
        target.GetStats().TakeDamage(damage, transform);
    }
}
