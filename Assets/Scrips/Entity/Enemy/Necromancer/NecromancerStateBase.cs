using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerStateBase : EnemyState
{
    protected Enemy_Necromancer enemy;
    public NecromancerStateBase(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Necromancer _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
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
