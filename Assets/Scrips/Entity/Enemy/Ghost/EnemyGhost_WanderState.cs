using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.AI;

public class EnemyGhost_WanderState : EnemyGhost_StateBase
{
    public EnemyGhost_WanderState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName,
        EnemyGhost _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetNavMeshAgentToWanderState();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemyBase.IsPlayerDetected() || enemyBase.IsDetectPlayerFront())
        {
            stateMachine.changeState(enemy.battleIdleState);
        }
    }
}
