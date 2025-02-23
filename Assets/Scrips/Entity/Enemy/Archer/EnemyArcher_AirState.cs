using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher_AirState : EnemyState
{
    Enemy_Archer enemy;
    public EnemyArcher_AirState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
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
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsGrounded())
        {
            enemy.SetVelocity(0, 0);
            stateMachine.changeState(enemy.idleState);
        }
    }
}
