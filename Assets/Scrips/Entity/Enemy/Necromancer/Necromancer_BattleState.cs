using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer_BattleState : NecromancerStateBase
{
    private Transform player;
    private int moveDir;

    public Necromancer_BattleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Necromancer _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
        
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
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        if(enemy.ShouldPullBack(moveDir != enemy.GetPlayerDir()))
        {
            moveDir = -enemy.GetPlayerDir();
        }
        else
        {
            moveDir = enemy.GetPlayerDir();
        }

        if ((enemy.IsTouchWall() || !enemy.IsGrounded()) && moveDir == enemy.facingDir)
        {
            enemy.FaceToPlayer();
            stateMachine.ChangeState(enemy.idleState);
        }
        else if (enemy.IsDetectPlayerFront() || enemy.IsPlayerDetected())
        {
            timer = enemy.battleDuration;
            enemy.SetVelocity(enemy.battleMoveSpeed * moveDir, enemy.rg.velocity.y);
            if (Vector2.Distance(enemy.transform.position, player.position) < enemy.toAttackRadius)
            {
                if (enemy.CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
            }
        }
        else
        {
            enemy.SetVelocity(enemy.battleMoveSpeed * moveDir, enemy.rg.velocity.y);
            if (timer < 0)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }
    }
}
