using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime_StunState : Enemytate
{
    Enemy_Slime enemy;
    public EnemySlime_StunState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
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
        if(enemy.IsGrounded())
        {
            enemy.anim.SetTrigger("StunGrounded");
        }
        if (timer < 0)
        {
            enemy.anim.ResetTrigger("StunGrounded");
            enemy.anim.SetTrigger("StunTimeOut");
        }
        if(triggerCalled)
        {
            enemy.anim.ResetTrigger("StunTimeOut");
            stateMachine.changeState(enemy.idleState);
        }
    }
}
