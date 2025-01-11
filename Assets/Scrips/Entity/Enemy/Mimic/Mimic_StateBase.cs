using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic_StateBase : EnemyState
{
    protected Enemy_Mimic enemy;
    public Mimic_StateBase(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Mimic _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
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
