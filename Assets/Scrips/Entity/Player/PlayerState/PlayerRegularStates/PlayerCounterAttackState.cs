using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(PlayerStateMachine _stateMachine, Player _Player, string _animBoolName) : base(_stateMachine, _Player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = player.counterAttackDuration;

        SceneAudioManager.instance.playerSFX.counterAttack.Play(null);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, 0);
        if(timer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackValidCheck.position, player.attackValidCheckRadius);
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                if(hit.GetComponent<Enemy>().TryToBeStuuned())
                {
                    stateMachine.ChangeState(player.successfulCounterAttackState);
                }
            }
            else if (hit.GetComponentInParent<Enemy>() != null)
            {
                if (hit.GetComponentInParent<Enemy>().TryToBeStuuned())
                {
                    stateMachine.ChangeState(player.successfulCounterAttackState);
                }
            }
            hit.GetComponent<IDeflectableProjectile>()?.BeDeflected();
        }
    }
}
