using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSuccessfulCounterAttackState : PlayerState
{
    public PlayerSuccessfulCounterAttackState(PlayerStateMachine _stateMachine, Player _Player, string _animBoolName) : base(_stateMachine, _Player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.intance.counterAttack.TryCreateClone_CounterAttack();
        SkillManager.intance.counterAttack.TryRecoverHealth_CounterAttack();

        SceneAudioManager.instance.playerSFX.counterAttackSuccess.Play(null);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, 0);
        if(isAnimFinish)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
