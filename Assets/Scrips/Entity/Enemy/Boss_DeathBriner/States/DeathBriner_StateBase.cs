using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBriner_StateBase: Enemytate
{
    protected EnemyBoss_DeathBriner enemy;
    public DeathBriner_StateBase(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyBoss_DeathBriner _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
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
    }
}
