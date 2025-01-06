using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Necromancer_ControllState : NecromancerStateBase
{
    public Necromancer_ControllState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Necromancer _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastAttackTime = Time.time;
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(0, 0);
        enemy.FaceToPlayer();
        if (enemy.currentSkull.IsDestroyed() || enemy.currentSkull.state != SkullState.Flying)
        {
            enemy.currentSkull = null;
            stateMachine.changeState(enemy.idleState);
        }
        else if(enemy.isTakingDamage)
        {
            enemy.currentSkull.Explode();
            enemy.currentSkull = null;
            stateMachine.changeState(enemy.idleState);
        }
    }
}
