using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhost_AttackState : EnemyGhost_StateBase
{
    public EnemyGhost_AttackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName,
        EnemyGhost _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(0, 0);
        enemy.Stop();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.changeState(enemy.battleIdleState);
        }
    }
}
