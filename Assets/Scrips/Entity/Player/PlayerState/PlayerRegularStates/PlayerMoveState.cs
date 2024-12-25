using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    private GroundType currentGroundType = GroundType.Rock;
    public PlayerMoveState(PlayerStateMachine _stateMachine, Player _Player, string _animBoolName) : base(_stateMachine, _Player, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlayerMove(currentGroundType);
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.PlayerStopMove(currentGroundType);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, rg.velocity.y);
        if (xInput == 0 || player.IsTouchWall())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
