using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic_DeadState : Mimic_StateBase
{
    public Mimic_DeadState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Mimic enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, enemy)
    {
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
