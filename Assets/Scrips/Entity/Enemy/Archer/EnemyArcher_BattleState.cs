using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher_BattleState : EnemyState
{
    Enemy_Archer enemy;
    private Transform player;
    private int moveDir;

    public EnemyArcher_BattleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        timer = enemy.battleDuration;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (PlayerManager.instance.player.isDead)
        {
            stateMachine.changeState(enemy.idleState);
            return;
        }

        if (player.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }
        else if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }

        if(enemy.shouldPullBack())
        {
            stateMachine.changeState(enemy.pullBackState);
        }
        else if ((enemy.IsTouchWall() || (!enemy.IsGrounded() && !enemy.IsPlatform())) && moveDir == enemy.facingDir)
        {
            stateMachine.changeState(enemy.idleState);
        }
        else if(enemy.IsDetectPlayerFront() || enemy.IsPlayerDetected())
        {
            timer = enemy.battleDuration;
            enemy.SetVelocity(enemy.battleMoveSpeed * moveDir, enemy.rg.velocity.y);
            if (Vector2.Distance(enemy.transform.position, player.position) < enemy.toAttackRadius)
            {
                if (enemy.CanAttack())
                {
                    stateMachine.changeState(enemy.attackState);
                }
            }
        }
        else
        {
            enemy.SetVelocity(enemy.battleMoveSpeed * moveDir, enemy.rg.velocity.y);
            if (timer < 0)
            {
                stateMachine.changeState(enemy.idleState);
            }
        }
    }
}
