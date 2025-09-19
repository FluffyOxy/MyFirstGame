using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher_AttackState : EnemyState
{
    Enemy_Archer enemy;
    public EnemyArcher_AttackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.lastAttackTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(0, 0);
        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
