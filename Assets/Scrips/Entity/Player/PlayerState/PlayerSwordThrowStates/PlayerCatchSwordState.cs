using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    public PlayerCatchSwordState(PlayerStateMachine _stateMachine, Player _Player, string _animBoolName) : base(_stateMachine, _Player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Transform sword = player.swordThrown.transform;
        if (sword.position.x > player.transform.position.x && player.isFacingLeft
            || sword.position.x < player.transform.position.x && !player.isFacingLeft
            )
        {
            player.Flip();
        }
        float feedback = SkillManager.intance.swordThrow.catchFeedback;
        player.SetVelocityWithoutFlip(feedback * -player.facingDir, rg.velocity.y);
        (player.fx as PlayerFX).SwordCatchFx();
        player.fx.ScreenShake(player.fx.shakePower_sword);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", player.unmovebleDurationAfterCatchSword);
    }

    public override void Update()
    {
        base.Update();
        if(isAnimFinish)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
