using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomberman_ExplodeHoldingState : Enemytate
{
    Enemy_Bomberman enemy;
    public EnemyBomberman_ExplodeHoldingState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Bomberman _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        timer = enemy.holdingDuration;
        Debug.Log("EnemyBomberman_ExplodeHoldingState");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(timer <= 0)
        {
            enemy.stateMachine.changeState(enemy.explodeState);
        }
    }
}
