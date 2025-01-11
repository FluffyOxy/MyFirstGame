using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic_BattleIdleState : Mimic_StateBase
{
    private Transform player;
    private int moveDir;

    public Mimic_BattleIdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Mimic enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        timer = enemy.battleDuration;
        enemy.SetVelocity(0, enemy.rg.velocity.y);

        Debug.Log("BattleIdle");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        moveDir = enemy.GetPlayerDir();

        if (CanCatchPlayer() && !IsTooCloseToPlayer())
        {
            stateMachine.changeState(enemy.battleState);
        }

        if (enemy.IsDetectPlayerFront())
        {
            timer = enemy.battleDuration;
            if (enemy.IsDetectPlayerFront().distance < enemy.toAttackRadius)
            {
                if (enemy.CanAttack())
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
