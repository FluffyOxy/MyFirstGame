using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBriner_DashAttackState : DeathBriner_AttackStateBase
{
    private float damageTimer;
    private float afterImageTimer;
    private int dashDir;
    private float defaultAnimSpeed;

    public DeathBriner_DashAttackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyBoss_DeathBriner _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void AttackTrigger()
    {
        
    }

    public override bool CanAttack()
    {
        return true;
    }

    public override void Enter()
    {
        base.Enter();
        damageTimer = 0;

        dashDir = -enemy.GetPullBackDir();
        defaultAnimSpeed = enemy.anim.speed;
        enemy.isDashing = false;

        enemy.SetCanBeDamage(false);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.anim.speed = defaultAnimSpeed;
        enemy.SetVelocity(0, enemy.rg.velocity.y);
        enemy.FaceToPlayer();

        damageTimer = 0;
        afterImageTimer = 0;

        enemy.SetCanBeDamage(true);
    }

    public override void Update()
    {
        base.Update();
        damageTimer -= Time.deltaTime;
        afterImageTimer -= Time.deltaTime;

        if(enemy.isDashing)
        {
            enemy.anim.speed = enemy.dashAnimPlaySpeed;

            if (damageTimer < 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackValidCheck.position, enemy.attackValidCheckRadius);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Player>() != null)
                    {
                        float damage = enemy.cs.DoDamageTo_PrimaryAttack(hit.GetComponent<Player>());
                        damageTimer = enemy.dashDamageCooldown;
                        break;
                    }
                }
            }

            enemy.SetVelocity(enemy.dashSpeed * dashDir, enemy.rg.velocity.y);

            if(afterImageTimer < 0)
            {
                afterImageTimer = enemy.fx.afterImageGenerateCooldown;
                enemy.fx.CreateAfterImage(!enemy.isFacingLeft);
            }
        }
        else
        {
            enemy.anim.speed = defaultAnimSpeed;
            enemy.SetVelocity(0, enemy.rg.velocity.y);
            enemy.FaceToPlayer();
        }
        
        if(triggerCalled || enemy.IsTouchWall() || !enemy.IsGrounded())
        {
            stateMachine.changeState(enemy.idleState);
        }
    }
}
