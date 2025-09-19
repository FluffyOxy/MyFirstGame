using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBriner_RunMoveState : DeathBriner_MoveStateBase
{
    int moveDir;
    Player player;
    public DeathBriner_RunMoveState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyBoss_DeathBriner _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player;
        timer = Random.Range(enemy.minMoveDuration, enemy.maxMoveDuration);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.transform.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if (player.transform.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }

        if(enemy.isPullBack)
        {
            moveDir = -moveDir;
        }

        if (Vector2.Distance(enemy.transform.position, player.transform.position) > enemy.toAttackRadius + Mathf.Abs(enemy.playerCheck.position.x - enemy.transform.position.x))
        {
            enemy.SetVelocity(enemy.battleMoveSpeed * moveDir, enemy.rg.velocity.y);
        }
        else
        {
            enemy.SetVelocity(0.1f * moveDir, enemy.rg.velocity.y);
        }

        if(enemy.currentAttackState.CanAttack())
        {
            stateMachine.ChangeState(enemy.currentAttackState as DeathBriner_AttackStateBase);
        }
        else if (timer < 0 || (enemy.IsTouchWall() && moveDir == enemy.facingDir) || (!enemy.IsGrounded() && moveDir == enemy.facingDir))
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

}
