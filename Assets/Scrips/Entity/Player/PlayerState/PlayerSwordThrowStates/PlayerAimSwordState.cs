using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(PlayerStateMachine _stateMachine, Player _Player, string _animBoolName) : base(_stateMachine, _Player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.intance.swordThrow.SetDotsActive(true);
        player.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
        SkillManager.intance.swordThrow.SetDotsActive(false);
        player.StartCoroutine("BusyFor", player.unmovebleDurationAfterThrowSword);

        SceneAudioManager.instance.playerSFX.swordThrow.Play(player.transform);
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, rg.velocity.y);
        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.idleState);
        }
        if(UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition).x > player.transform.position.x && player.isFacingLeft
            || UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition).x < player.transform.position.x && !player.isFacingLeft
            )
        {
            player.Flip();
        }
    }
}
