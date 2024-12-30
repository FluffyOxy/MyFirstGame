using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime_AttackState : Enemytate
{
    protected Enemy_Slime enemy;
    public EnemySlime_AttackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastAttackTime = Time.time;
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(0, 0);
        if (triggerCalled)
        {
            stateMachine.changeState(enemy.battleState);
        }
    }
}
