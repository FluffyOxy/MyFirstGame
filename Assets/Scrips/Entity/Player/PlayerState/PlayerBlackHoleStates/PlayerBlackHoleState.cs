using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private bool isUsed;

    public PlayerBlackHoleState(PlayerStateMachine _stateMachine, Player _Player, string _animBoolName) : base(_stateMachine, _Player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = SkillManager.intance.blackHole.flyTime;
        rg.gravityScale = 0;
        isUsed = false;
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, 0);
        player.SetGravityToDefault();
    }

    public override void Update()
    {
        base.Update();
        if(timer > 0)
        {
            player.SetVelocity(0, SkillManager.intance.blackHole.flyUpSpeed);
        }
        else
        {
            player.SetVelocity(0, -SkillManager.intance.blackHole.flyDownSpeed);
            if (!isUsed)
            {
                isUsed = true;
                SkillManager.intance.blackHole.createBlackHole();
            }
        }

        if(isUsed && SkillManager.intance.blackHole.canPlayerExitState())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
