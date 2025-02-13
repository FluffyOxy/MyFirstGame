using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic_Animation : EnemyAnimation
{
    public void OpenTrigger()
    {
        enemy.DropItem();
    }

    protected override void AttackTrigger()
    {
        base.AttackTrigger();

        SceneAudioManager.instance.mimicSFX.attack.Play(enemy.transform);
    }
}
