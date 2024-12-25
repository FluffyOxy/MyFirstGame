using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime_MoveState : EnemySlime_GroundedState
{
    public EnemySlime_MoveState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
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
        if (enemy.IsTouchWall())
        {
            enemy.SetVelocity(enemy.moveSpeed * -enemy.facingDir, enemy.rg.velocity.y);
        }
        else if (!enemy.IsGrounded())
        {
            stateMachine.changeState(enemy.idleState);
        }
        else
        {
            enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rg.velocity.y);
        }
        Debug.Log(enemy.rg.velocity);
        Debug.Log("enemy.moveSpeed: " + enemy.moveSpeed);
        Debug.Log("enemy.facingDir: " + enemy.facingDir);

        if (enemy.IsDetectPlayerFront() || enemy.IsPlayerNearBy())
        {
            stateMachine.changeState(enemy.battleState);
        }
    }
}
