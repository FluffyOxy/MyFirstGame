using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBrinerAnimation : EnemyAnimation
{
    protected override void AttackTrigger()
    {
        (enemy as EnemyBoss_DeathBriner)?.currentAttackState.AttackTrigger();
    }

    protected void FlashOutTrigger()
    {
        (enemy as EnemyBoss_DeathBriner).isFlashOut = true;

        SceneAudioManager.instance.deathBrinerSFX.flash.Play(enemy.transform);
    }

    protected void FlashInTrigger()
    {
        AnimFinishTrigger();
    }

    protected void DashStartTrigger()
    {
        (enemy as EnemyBoss_DeathBriner).isDashing = true;

        SceneAudioManager.instance.deathBrinerSFX.dash.Play(enemy.transform);
    }
    protected void DashEndTrigger()
    {
        (enemy as EnemyBoss_DeathBriner).isDashing = false;
    }
}
