using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : Enemytate
{
    private Enemy_Skeleton enemy;
    private Transform player;
    private int moveDir;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = enemy;
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
            stateMachine.changeState(enemy.idleState);
        }
        else if(Vector2.Distance(enemy.transform.position, player.position) > enemy.toAttackRadius + Mathf.Abs(enemy.playerCheck.position.x - enemy.transform.position.x))
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
                if(CanAttack())
                {
                    stateMachine.changeState(enemy.attackState);
                }
            }
        }
        else
        {
            if (timer < 0)
            {
                stateMachine.changeState(enemy.idleState);
            }
        }

        if (PlayerManager.instance.player.isDead)
        {
            stateMachine.changeState(enemy.idleState);
        }
    }

    private bool CanAttack()
    {
        return (Time.time - enemy.lastAttackTime) > Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
    }
}
