using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DeathBriner_AttackStateBase : DeathBriner_StateBase, IDeathBrinerAttackState
{
    public DeathBriner_AttackStateBase(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyBoss_DeathBriner _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public virtual void AttackTrigger()
    {
        
    }

    public virtual bool CanAttack()
    {
        return true;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.SetRandomIdleDuration();
        enemy.CloseCounterAttackWindow();
    }

    public override void Update()
    {
        base.Update();
    }
}
