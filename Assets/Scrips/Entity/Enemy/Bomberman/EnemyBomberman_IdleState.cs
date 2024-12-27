using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomberman_IdleState : EnemyState
{
    Enemy_Bomberman enemy;
    public EnemyBomberman_IdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Bomberman _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
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
        if (enemy.IsDetectPlayerFront() || enemy.IsPlayerDetected())
        {
            float moveDir = 1;
            if (PlayerManager.instance.player.transform.position.x < enemy.transform.position.x)
            {
                moveDir = -1;
            }
            if ((enemy.IsTouchWall() || !enemy.IsGrounded()) && moveDir == enemy.facingDir)
            {
                timer = Random.Range(enemy.minIdleDuration, enemy.maxIdleDuration);
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
