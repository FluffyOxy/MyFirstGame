using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhost_StateBase : EnemyState
{
    protected EnemyGhost enemy;
    public EnemyGhost_StateBase(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyGhost _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        enemy = _enemy;
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
        enemy.FlipCheck();
    }
}
