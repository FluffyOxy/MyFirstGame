using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomberman_Animation : EnemyAnimation
{
    public void ExplodeTrigger()
    {
        (enemy as Enemy_Bomberman).isExplode = true;

        SceneAudioManager.instance.bombermanSFX.explode.Play(enemy.transform);
    }

    public void ExplodeFinishTrigger()
    {
        enemy.stateMachine.changeState((enemy as Enemy_Bomberman).deadState);
        (enemy as Enemy_Bomberman).EnterDeadState();
    }

    protected override void AttackTrigger()
    {
        base.AttackTrigger();

        SceneAudioManager.instance.bombermanSFX.attack.Play(enemy.transform);
    }
}
