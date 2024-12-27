using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher_PullBackJumpState : EnemyState
{
    Enemy_Archer enemy;
    public EnemyArcher_PullBackJumpState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(enemy.GetPullBackDir() * enemy.pullBackJumpForce.x, enemy.pullBackJumpForce.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastPullBackJumpTime = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if(enemy.rg.velocity.y <= 0)
        {
            enemy.stateMachine.changeState(enemy.airState);
        }
    }
}
