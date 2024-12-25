using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (!enemy.IsGrounded())
        {
            enemy.SetVelocity(enemy.moveSpeed * -enemy.facingDir, enemy.rg.velocity.y);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(enemy.IsTouchWall())
        {
            enemy.SetVelocity(enemy.moveSpeed * -enemy.facingDir, enemy.rg.velocity.y);
        }
        else if(!enemy.IsGrounded())
        {
            stateMachine.changeState(enemy.idleState);
        }
        else
        {
            enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rg.velocity.y);
        }

        if (enemy.IsDetectPlayerFront() || enemy.IsPlayerNearBy())
        {
            stateMachine.changeState(enemy.battleState);
        }
    }
}
