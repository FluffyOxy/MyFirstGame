using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomberman_BattleState : EnemyState
{
    Enemy_Bomberman enemy;
    private Transform player;
    private int moveDir;

    public EnemyBomberman_BattleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Bomberman _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
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
        if (player.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }
        else if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }

        if ((enemy.IsTouchWall() || !enemy.IsGrounded()) && moveDir == enemy.facingDir)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
        else if (Vector2.Distance(enemy.transform.position, player.position) > enemy.toAttackRadius + Mathf.Abs(enemy.playerCheck.position.x - enemy.transform.position.x))
        {
            enemy.SetVelocity(enemy.battleMoveSpeed * moveDir, enemy.rg.velocity.y);
        }
        else
        {
            enemy.SetVelocity(0, enemy.rg.velocity.y);
        }

        if (enemy.IsDetectPlayerFront())
        {
            timer = enemy.battleDuration;
            if (enemy.IsDetectPlayerFront().distance < enemy.toAttackRadius)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
            }
        }
        else
        {
            if (timer < 0)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        if (PlayerManager.instance.player.isDead)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
    private bool CanAttack()
    {
        return (Time.time - enemy.lastAttackTime) > Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
    }
}