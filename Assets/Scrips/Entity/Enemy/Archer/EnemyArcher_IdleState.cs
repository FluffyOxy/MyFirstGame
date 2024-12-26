using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher_IdleState : EnemyArcher_GroundedState
{
    public EnemyArcher_IdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = Random.Range(enemy.minIdleDuration, enemy.maxIdleDuration);
        enemy.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.shouldPullBack())
        {
            stateMachine.changeState(enemy.pullBackState);
        }
        else if (enemy.IsDetectPlayerFront() || enemy.IsPlayerDetected())
        {
            float moveDir = 1;
            if (PlayerManager.instance.player.transform.position.x < enemy.transform.position.x)
            {
                moveDir = -1;
            }

            if ((enemy.IsTouchWall() || !enemy.IsGrounded()) && moveDir == enemy.facingDir)
            {
                timer = Random.Range(enemy.minIdleDuration, enemy.maxIdleDuration);
                if (Vector2.Distance(enemy.transform.position, PlayerManager.instance.player.transform.position) < enemy.toAttackRadius)
                {
                    if (enemy.CanAttack())
                    {
                        stateMachine.changeState(enemy.attackState);
                    }
                }
            }
            else
            {
                stateMachine.changeState(enemy.battleState);
            }

        }
        if (timer < 0)
        {
            stateMachine.changeState(enemy.moveState);
        }
    }
}
