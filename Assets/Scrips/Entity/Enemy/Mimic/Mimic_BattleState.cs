using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic_BattleState : Mimic_StateBase
{
    private Transform player;
    private int moveDir;

    public Mimic_BattleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Mimic _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
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

        moveDir = enemy.GetPlayerDir();

        if (!CanCatchPlayer() || IsTooCloseToPlayer())
        {
            stateMachine.ChangeState(enemy.battleIdleState);
        }
        else
        {
            enemy.SetVelocity(enemy.battleMoveSpeed * moveDir, enemy.rg.velocity.y);
        }

        if (enemy.IsDetectPlayerFront())
        {
            timer = enemy.battleDuration;
            if (enemy.IsDetectPlayerFront().distance < enemy.toAttackRadius)
            {
                if (enemy.CanAttack())
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
    }

    private bool CanCatchPlayer()
    {
        return !((enemy.IsTouchWall() || !enemy.IsGrounded()) && moveDir == enemy.facingDir);
    }

    private bool IsTooCloseToPlayer()
    {
        return Vector2.Distance(enemy.transform.position, player.position) <
            enemy.toAttackRadius + Mathf.Abs(enemy.playerCheck.position.x - enemy.transform.position.x);
    }
}
