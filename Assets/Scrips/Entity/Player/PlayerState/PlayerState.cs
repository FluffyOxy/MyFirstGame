using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    private string animBoolName;

    protected float xInput;
    protected float yInput;
    protected Rigidbody2D rg;

    protected float timer;
    protected bool isAnimFinish;

    public PlayerState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.player = _player;
        animBoolName = _animBoolName;
    }

    public virtual void Enter() 
    {
        player.anim.SetBool(animBoolName, true);
        rg = player.rg;
        isAnimFinish = false;
    }

    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        timer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public void AnimFinishTrigger()
    {
        isAnimFinish = true;
    }
}
