using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher_PullBackState : EnemyState
{
    Enemy_Archer enemy;
    private int pullBackDir;
    public EnemyArcher_PullBackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        pullBackDir = enemy.GetPullBackDir();
        if(!enemy.TryPullBackJump())
        {
            enemy.SetVelocity(enemy.pullBackSpeed * pullBackDir, enemy.rg.velocity.y);
        }
        else
        {
            return;
        }

        if (ShouldStopPullBack())
        {
            enemy.stateMachine.changeState(enemy.battleState);
        }
        else if (Vector2.Distance(enemy.transform.position, PlayerManager.instance.player.transform.position) < enemy.toAttackRadius)
        {
            if (enemy.CanAttack())
            {
                stateMachine.changeState(enemy.attackState);
            }
        }

        if((enemy.IsTouchWall() || !enemy.IsGrounded()) && pullBackDir == enemy.facingDir)
        {
            enemy.SetVelocity(0, enemy.rg.velocity.y);
        }
    }


    private bool ShouldStopPullBack()
    {
        Vector3 playerPosition = PlayerManager.instance.player.transform.position;
        return Vector2.Distance(playerPosition, enemy.transform.position) >= enemy.toAttackRadius;
    }
}
