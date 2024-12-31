using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBriner_JumpState : DeathBriner_MoveStateBase
{
    public DeathBriner_JumpState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyBoss_DeathBriner _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocityWithoutFlip(enemy.pullBackJumpForce.x * enemy.jumpDir, enemy.pullBackJumpForce.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastPullBackJumpTime = Time.time;
        enemy.idleDuration = 0;
    }

    public override void Update()
    {
        base.Update();
        enemy.FaceToPlayer();
        if (enemy.IsGrounded() && enemy.rg.velocity.y <= 0)//这个方案可能在以下情况导致bug：当其站在一个向上移动的平台上时，enemy的velocity.y还大于0时就触地
        {
            enemy.SetVelocity(0, 0);
            stateMachine.changeState(enemy.idleState);
        }
    }
}
