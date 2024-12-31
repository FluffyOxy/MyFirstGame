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
    }

    protected void FlashInTrigger()
    {
        AnimFinishTrigger();
    }

    protected void DashStartTrigger()
    {
        (enemy as EnemyBoss_DeathBriner).isDashing = true;
    }
    protected void DashEndTrigger()
    {
        (enemy as EnemyBoss_DeathBriner).isDashing = false;
    }
}
