using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic_MoveState : Mimic_StateBase
{
    public Mimic_MoveState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Mimic enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, enemy)
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
            stateMachine.ChangeState(enemy.idleState);
        }
        else
        {
            enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rg.velocity.y);
        }

        if (enemy.IsDetectPlayerFront() || enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.battleIdleState);
        }
    }
}
