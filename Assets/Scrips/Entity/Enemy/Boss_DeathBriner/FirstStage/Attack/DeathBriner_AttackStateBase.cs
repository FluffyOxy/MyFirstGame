using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DeathBriner_AttackStateBase : DeathBriner_FirstStageState, IDeathBrinerAttackState
{
    public DeathBriner_AttackStateBase(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyBoss_DeathBriner _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public virtual void AttackTrigger()
    {
        Assert.IsTrue(false, "AttackStateBase.AttackDamage() 不应被调用！");
    }

    public virtual bool CanAttack()
    {
        Assert.IsTrue(false, "AttackStateBase.CanAttack() 不应被调用！");
        return false;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.SetRandomIdleDuration();
    }

    public override void Update()
    {
        base.Update();
    }
}
