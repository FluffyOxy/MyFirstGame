using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBriner_DeadState : DeathBriner_StateBase
{
    public DeathBriner_DeadState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyBoss_DeathBriner _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
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
