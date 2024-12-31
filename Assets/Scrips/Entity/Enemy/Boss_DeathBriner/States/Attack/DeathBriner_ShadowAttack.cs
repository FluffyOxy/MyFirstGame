using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

enum ShadowState
{
    Enter, 
    Exit,
    AttackIdle,
    Attack,
    LeaveIdle,
    Leave
}
public class DeathBriner_ShadowAttack : DeathBriner_AttackStateBase
{
    ShadowState state;
    private int attackCount;

    private bool isTarget;
    private Vector3 dashTarget;

    private float shadowDamageTimer;
    private float shadowIdleTimer;
    public DeathBriner_ShadowAttack(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyBoss_DeathBriner _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        attackCount = Random.Range(enemy.minShadowAttackAmount,enemy.maxShadowAttackAmount);

        enemy.rg.isKinematic = true;

        shadowDamageTimer = 0;
        shadowIdleTimer = 0;

        enemy.shadowParticle.Play();

        isTarget = false;

        state = ShadowState.Enter;

        enemy.SetShadowStunCollider(true);

        enemy.canBeDamage = false;
    }

    public override void Exit()
    {
        base.Exit();
        
        enemy.rg.isKinematic = false;
        enemy.shadowParticle.Stop();

        enemy.SetShadowStunCollider(false);

        enemy.canBeDamage = true;
    }

    public override void Update()
    {
        base.Update();
        shadowDamageTimer -= Time.deltaTime;
        shadowIdleTimer -= Time.deltaTime;

        switch (state)
        {
            case ShadowState.Enter:
                {
                    enemy.SetVelocity(0, 0);
                    if(enemy.isFlashOut)
                    {
                        state = ShadowState.Attack;
                    }
                    break;
                }
            case ShadowState.Attack:
                {
                    if (!isTarget)
                    {
                        dashTarget = PlayerManager.instance.player.transform.position;
                        isTarget = true;
                        enemy.OpenCounterAttackWindow();
                    }

                    enemy.transform.position =
                            Vector2.MoveTowards(enemy.transform.position, dashTarget, enemy.shadowMoveSpeed * Time.deltaTime);

                    if (Vector2.Distance(enemy.transform.position, dashTarget) < 0.1f)
                    {
                        isTarget = false;

                        state = ShadowState.AttackIdle;
                        shadowIdleTimer = Random.Range(enemy.minShadowIdleDuration, enemy.maxShadowIdleDuration);
                        enemy.CloseCounterAttackWindow();
                    }
                    break;
                }
            case ShadowState.AttackIdle:
                {
                    if(shadowIdleTimer < 0)
                    {
                        state = ShadowState.Leave;
                        enemy.SetVelocity(0, enemy.shadowIdleMoveUpSpeed);
                    }
                    break;
                }
            case ShadowState.Leave:
                {
                    if (!isTarget)
                    {
                        float randomX =
                            Random.Range(enemy.flashRemoteAttackPosition[0].position.x, enemy.flashRemoteAttackPosition[1].position.x);
                        float randomY = enemy.flashRemoteAttackPosition[0].position.y; 
                        dashTarget = new Vector3(randomX, randomY);
                        isTarget = true;
                        enemy.OpenCounterAttackWindow();
                    }

                    enemy.transform.position =
                            Vector2.MoveTowards(enemy.transform.position, dashTarget, enemy.shadowMoveSpeed * Time.deltaTime);

                    if (Vector2.Distance(enemy.transform.position, dashTarget) < 0.1f)
                    {
                        isTarget = false;

                        --attackCount;
                        if (attackCount > 0)
                        {
                            state = ShadowState.LeaveIdle;
                            shadowIdleTimer = Random.Range(enemy.minShadowIdleDuration, enemy.maxShadowIdleDuration);
                        }
                        else
                        {
                            state = ShadowState.Exit;
                            enemy.anim.SetTrigger("FlashIn");
                            enemy.isFlashOut = false;
                        }
                        enemy.CloseCounterAttackWindow();
                    }
                    break;
                }
            case ShadowState.LeaveIdle:
                {
                    if (shadowIdleTimer < 0)
                    {
                        state = ShadowState.Attack;
                        enemy.SetVelocity(0, enemy.shadowIdleMoveUpSpeed);
                    }
                    break;
                }
            case ShadowState.Exit:
                {
                    enemy.SetVelocity(0, 0);
                    break;
                }
        }

        if (shadowDamageTimer < 0 && state != ShadowState.Enter && state != ShadowState.Exit)
        {
            Collider2D[] colliders =
            Physics2D.OverlapCircleAll(enemy.shadowParticle.transform.position, enemy.shadowDamageRadius, enemy.whatIsPlayer);
            foreach (Collider2D hit in colliders)
            {
                if (hit.GetComponent<Player>() != null)
                {
                    float damage = enemy.cs.DoDamageTo_PrimaryAttack(hit.GetComponent<Player>());
                    shadowDamageTimer = enemy.shadowDamageCooldown;
                }
            }
        }

        if (triggerCalled)
        {
            stateMachine.changeState(enemy.idleState);
        }
    }

    public void BeStunned()
    {
        state = ShadowState.Exit;
        enemy.anim.SetTrigger("FlashIn");
        enemy.isFlashOut = false;
    }
}
