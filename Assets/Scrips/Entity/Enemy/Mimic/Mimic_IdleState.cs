using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic_IdleState : Mimic_StateBase
{
    public Mimic_IdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Mimic _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = Random.Range(enemy.minIdleDuration, enemy.maxIdleDuration);
        enemy.SetVelocity(0, enemy.rg.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsDetectPlayerFront() || enemy.IsPlayerDetected())
        {
            stateMachine.changeState(enemy.battleIdleState);
        }
        else if (timer < 0)
        {
            stateMachine.changeState(enemy.moveState);
        }
    }
}
