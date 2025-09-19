using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomberman_AttackState : EnemyState
{
    Enemy_Bomberman enemy;
    public EnemyBomberman_AttackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Bomberman _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
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
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
