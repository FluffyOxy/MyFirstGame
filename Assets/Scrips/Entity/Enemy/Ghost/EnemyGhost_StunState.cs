using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhost_StunState : EnemyGhost_StateBase
{
    public EnemyGhost_StunState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName,
        EnemyGhost _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        timer = enemy.stunDuration;
        enemy.SetVelocityWithoutFlip(enemy.stunDir.x * -enemy.facingDir, enemy.stunDir.y);
        enemy.fx.InvokeRepeating("RedColerBlink", 0, enemy.fx.stunBlinkRate);
        PlayerManager.instance.player.DoDamageTo_CounterAttack(enemy);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.fx.Invoke("CancelColerBlink", 0);
    }

    public override void Update()
    {
        base.Update();

        if (timer < 0)
        {
            stateMachine.ChangeState(enemy.battleIdleState);
        }
    }
}
