using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhost_BattleState : EnemyGhost_StateBase
{
    private float battleStateTimer;
    private float attackCooldownTimer;

    public EnemyGhost_BattleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName,
        EnemyGhost _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetNavMeshAgentToBattleState();
        ResetBattleStateTimer();
        ResetAttackCooldownTimer();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        battleStateTimer -= Time.deltaTime;
        attackCooldownTimer -= Time.deltaTime;

        if (enemy.CanSeePlayer() && (enemy.IsPlayerDetected() || enemy.IsDetectPlayerFront()))
        {
            ResetBattleStateTimer();
            if (attackCooldownTimer < 0)
            {
                stateMachine.changeState(enemy.attackState);
                ResetAttackCooldownTimer();
            }
        }
        else if (battleStateTimer < 0)
        {
            stateMachine.changeState(enemy.idleState);
        }
    }

    protected void ResetBattleStateTimer()
    {
        battleStateTimer = enemy.battleDuration;
    }

    protected void ResetAttackCooldownTimer()
    {
        float cooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
        attackCooldownTimer = cooldown;
    }
}
