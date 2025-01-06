using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Necromancer_AttackState : NecromancerStateBase
{

    public Necromancer_AttackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Necromancer _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.isAnimSkullThrowTrigger = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(0, 0);
        enemy.FaceToPlayer();

        if(enemy.isAnimSkullThrowTrigger)
        {
            enemy.isAnimSkullThrowTrigger = false;
            enemy.throwSkullToPlayer();
        }

        if (triggerCalled)
        {
            stateMachine.changeState(enemy.controllingState);
        }
    }
}
