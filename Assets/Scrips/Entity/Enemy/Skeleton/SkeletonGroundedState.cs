using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : Enemytate
{
    protected Enemy_Skeleton enemy;
    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
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
    }
}
