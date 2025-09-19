using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic_WatchState : Mimic_StateBase
{
    public Mimic_WatchState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Mimic enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, enemy)
    {
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
        if(triggerCalled)
        {
            stateMachine.ChangeState(enemy.chestState);
        }
    }
}
