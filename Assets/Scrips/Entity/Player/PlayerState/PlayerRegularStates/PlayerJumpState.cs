using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerStateMachine _stateMachine, Player _Player, string _animBoolName) : base(_stateMachine, _Player, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(rg.velocity.x, player.jumpSpeed);
        AudioManager.instance.PlayerJump();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(rg.velocity.y < 0)
        {
            stateMachine.ChangeState(player.fallState);
        }

    }
}
