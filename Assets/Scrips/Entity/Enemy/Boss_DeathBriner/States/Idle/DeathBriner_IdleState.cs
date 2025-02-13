using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBriner_IdleState : DeathBriner_StateBase
{
    bool isFighting = false;

    public DeathBriner_IdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyBoss_DeathBriner _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = enemy.idleDuration;
        enemy.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
        SceneAudioManager.instance.deathBrinerSFX.roar.Play(enemy.transform);
    }

    public override void Update()
    {
        base.Update();
        if(!isFighting)
        {
            if(enemy.IsPlayerDetected() || enemy.IsDetectPlayerFront())
            {
                isFighting = true;
                PlanToAttack();
            }
        }
        else
        {
            if(timer < 0)
            {
                PlanToAttack();
            }
            else if(enemy.IsGrounded() && enemy.ShouldPullBackJump())
            {
                if(enemy.CanJump())
                {
                    enemy.CalculatePullBackJumpParameter();
                    stateMachine.changeState(enemy.jumpState);
                }
            }
        }
    }

    private void PlanToAttack()
    {
        Player player = PlayerManager.instance.player;
        float dice = Random.Range(0, 100);
        float rate = 0;

        if(enemy.isStageChanging)
        {
            enemy.currentAttackState = enemy.remoteAttackState;
            enemy.isPullBack = true;
            enemy.SetRandomAttackCount();
            stateMachine.changeState(enemy.runMoveState);
            return;
        }

        //闪现近战攻击
        if (dice < enemy.flashAttackRate_primary + rate)
        {
            rate += enemy.flashAttackRate_primary;
            int moveDir = -enemy.GetPullBackDir();
            float moveDistance = 
                Mathf.Abs((player.transform.position.x + enemy.toAttackRadius * -player.facingDir) - enemy.transform.position.x);

            if (enemy.IsMoveSafe(moveDir, moveDistance))
            {
                enemy.flashMoveState.targetPosition = 
                    new Vector3(player.transform.position.x + enemy.toAttackRadius / 2 * -player.facingDir, enemy.transform.position.y);
            }
            else
            {
                enemy.flashMoveState.targetPosition =
                    new Vector3(player.transform.position.x + enemy.toAttackRadius / 2 * player.facingDir, enemy.transform.position.y);
            }

            enemy.currentAttackState = enemy.primaryAttackState;
            enemy.SetRandomAttackCount();
            stateMachine.changeState(enemy.flashMoveState);
        }
        //闪现远程攻击
        else if(dice < enemy.flashAttackRate_remote + (rate += enemy.flashAttackRate_primary))
        {
            float distanceToLeft = Vector2.Distance(player.transform.position, enemy.flashRemoteAttackPosition[0].position);
            float distanceToRight = Vector2.Distance(player.transform.position, enemy.flashRemoteAttackPosition[1].position);

            if(distanceToLeft > distanceToRight)
            {
                enemy.flashMoveState.targetPosition = enemy.flashRemoteAttackPosition[0].position;
            }
            else
            {
                enemy.flashMoveState.targetPosition = enemy.flashRemoteAttackPosition[1].position;
            }

            enemy.currentAttackState = enemy.remoteAttackState;
            enemy.SetRandomAttackCount();
            stateMachine.changeState(enemy.flashMoveState);
        }
        //冲刺攻击
        else if(dice < enemy.dashAttackRate + (rate += enemy.flashAttackRate_remote))
        {
            enemy.currentAttackState = enemy.dashAttackState;
            enemy.attackCounter = 1;
            stateMachine.changeState(enemy.dashAttackState);
        }
        //暗影攻击
        else if(dice < enemy.shadowAttackRate + (rate += enemy.dashAttackRate))
        {
            enemy.currentAttackState = enemy.shadowAttackState;
            enemy.attackCounter = 1;
            stateMachine.changeState(enemy.shadowAttackState);
        }
        //普通的近战和远程攻击
        else
        {
            bool isPlayerMoveToEnemy = enemy.IsPlayerFaceToEnemy();
            if (enemy.primaryAttackState.CanAttack() || (enemy.IsPlayerDetected() && isPlayerMoveToEnemy))
            {
                enemy.currentAttackState = enemy.primaryAttackState;
                enemy.isPullBack = false;
                enemy.SetRandomAttackCount();
                stateMachine.changeState(enemy.runMoveState);
            }
            else if (enemy.remoteAttackState.CanAttack() || (enemy.IsPlayerDetected() && !isPlayerMoveToEnemy))
            {
                enemy.currentAttackState = enemy.remoteAttackState;
                enemy.isPullBack = true;
                enemy.SetRandomAttackCount();
                stateMachine.changeState(enemy.runMoveState);
            }
        }
    }
}
