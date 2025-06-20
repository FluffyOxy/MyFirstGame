using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhost_BattleFlyState : EnemyGhost_BattleState
{
    private bool isSetDestinationThisFrame = false;

    public EnemyGhost_BattleFlyState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName,
        EnemyGhost _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetDestination(PlayerManager.instance.player.transform.position);
        isSetDestinationThisFrame = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (enemy.CanSeePlayer() && enemy.CheckDistanceToPlayer() < enemy.attackValidCheckRadius)
        {
            stateMachine.changeState(enemy.battleIdleState);
        }
        else
        {
            enemy.SetDestination(PlayerManager.instance.player.transform.position);
        }

        if (!isSetDestinationThisFrame && !enemy.IsDestinationValid())
        {
            stateMachine.changeState(enemy.battleIdleState);
        }
        isSetDestinationThisFrame = false;

        base.Update();
    }
}
