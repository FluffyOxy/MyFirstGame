using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhost_BattleIdleState : EnemyGhost_BattleState
{
    public EnemyGhost_BattleIdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName,
        EnemyGhost _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(0, 0);
        enemy.Stop();

        Debug.Log("EnemyGhost_BattleIdleState");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (!enemy.CanSeePlayer() || enemy.CheckDistanceToPlayer() > enemy.attackValidCheckRadius)
        {
            stateMachine.changeState(enemy.battleFlyState);
        }

        base.Update();
    }
}
