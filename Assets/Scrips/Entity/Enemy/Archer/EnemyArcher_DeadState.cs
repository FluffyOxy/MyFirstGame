using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher_DeadState : Enemytate
{
    Enemy_Archer enemy;
    public EnemyArcher_DeadState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        timer = enemy.deadAnimDuration;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (timer > 0)
        {
            enemy.rg.velocity = enemy.deadAnimVelocity;
        }
    }
}
