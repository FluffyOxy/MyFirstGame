using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBriner_FlashMoveState : DeathBriner_MoveStateBase
{
    public Vector3 targetPosition;

    public DeathBriner_FlashMoveState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyBoss_DeathBriner _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.canBeDamage = false;
        enemy.anim.ResetTrigger("FlashIn");
        enemy.isFlashOut = false;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.canBeDamage = true;
    }

    public override void Update()
    {
        base.Update();
        if(enemy.isFlashOut)
        {
            enemy.transform.position = targetPosition;
            enemy.anim.SetTrigger("FlashIn");
            enemy.isFlashOut = false;
        }
        if(triggerCalled)
        {
            stateMachine.changeState(enemy.currentAttackState as DeathBriner_AttackStateBase);
        }
    }
}
