using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimation : EnemyAnimation
{
    protected override void AttackTrigger()
    {
        base.AttackTrigger();

        SceneAudioManager.instance.skeletonSFX.attack.Play(enemy.transform);
    }
}
