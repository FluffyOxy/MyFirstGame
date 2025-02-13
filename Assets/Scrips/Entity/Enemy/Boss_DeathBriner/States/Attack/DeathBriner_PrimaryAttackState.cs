using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBriner_PrimaryAttackState : DeathBriner_AttackStateBase
{
    public DeathBriner_PrimaryAttackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyBoss_DeathBriner _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
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
        if (triggerCalled)
        {
            if(enemy.attackCounter > 1)
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

    public override bool CanAttack()
    {
        if (enemy.IsDetectPlayerFront())
        {
            if (enemy.IsDetectPlayerFront().distance < enemy.toAttackRadius)
            {
                return true;
            }
        }
        return false;
    }

    public override void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackValidCheck.position, enemy.attackValidCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                float damage = enemy.cs.DoDamageTo_PrimaryAttack(hit.GetComponent<Player>());
            }
        }

        SceneAudioManager.instance.deathBrinerSFX.attack.Play(enemy.transform);
    }
}
