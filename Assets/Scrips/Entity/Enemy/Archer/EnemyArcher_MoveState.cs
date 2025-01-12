using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher_MoveState : EnemyArcher_GroundedState
{
    public EnemyArcher_MoveState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (!enemy.IsGrounded() && !enemy.IsPlatform())
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
        if (enemy.IsTouchWall())
        {
            enemy.SetVelocity(enemy.moveSpeed * -enemy.facingDir, enemy.rg.velocity.y);
        }
        else if (!enemy.IsGrounded() && !enemy.IsPlatform())
        {
            stateMachine.changeState(enemy.idleState);
        }
        else
        {
            enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rg.velocity.y);
        }

        if(enemy.shouldPullBack())
        {
            stateMachine.changeState(enemy.pullBackState);
        }
        else if (enemy.IsDetectPlayerFront() || enemy.IsPlayerDetected())
        {
            stateMachine.changeState(enemy.battleState);
        }
    }
}
