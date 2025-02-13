using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlimeAnimation : EnemyAnimation
{
    protected override void AttackTrigger()
    {
        base.AttackTrigger();

        SceneAudioManager.instance.slimeSFX.attack.Play(enemy.transform);
    }
}
