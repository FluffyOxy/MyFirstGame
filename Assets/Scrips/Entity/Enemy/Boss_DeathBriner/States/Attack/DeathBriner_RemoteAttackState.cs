using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBriner_RemoteAttackState : DeathBriner_AttackStateBase
{
    public DeathBriner_RemoteAttackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyBoss_DeathBriner _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void AttackTrigger()
    {
        enemy.GenerateDeathHand();
    }

    public override bool CanAttack()
    {
        if (!enemy.IsPlayerDetected())
        {
            return true;
        }
        return false;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.FaceToPlayer();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(0, 0);
        if(enemy.isTakingDamage)
        {
            stateMachine.changeState(enemy.idleState);
        }
        else if (triggerCalled)
        {
            if (enemy.attackCounter > 1)
            {
                --enemy.attackCounter;
                stateMachine.changeState(this);
            }
            else
            {
                stateMachine.changeState(enemy.idleState);
            }
        }
    }
}
