using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhost_IdleState : EnemyGhost_WanderState
{
    public EnemyGhost_IdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyGhost _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }
    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(0, 0);
        enemy.Stop();
        timer = Random.Range(enemy.minIdleDuration, enemy.maxIdleDuration);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (timer < 0)
        {
            stateMachine.ChangeState(enemy.flyState);
        }

        base.Update();
    }
}
