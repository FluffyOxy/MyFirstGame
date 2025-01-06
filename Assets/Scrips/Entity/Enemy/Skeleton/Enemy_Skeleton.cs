using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    #region States
    public EnemyState idleState { get; private set; }
    public EnemyState moveState { get; private set; }
    public EnemyState battleState { get; private set; }
    public EnemyState attackState { get; private set; }
    public EnemyState stunnedState { get; private set; }
    public EnemyState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        battleState = new SkeletonBattleState(this, stateMachine, "Move", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SkeletonStunState(this, stateMachine, "Stunned", this);
        deadState = new SkeletonDeadState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        overlapCheckRadius = GetComponent<CapsuleCollider2D>().size.x;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool TryToBeStuuned()
    {
        if(base.TryToBeStuuned())
        {
            stateMachine.changeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void DamageSourceNotice(Entity _damageSource)
    {
        if (_damageSource != null)
        {
            Player player = _damageSource as Player;
            if (player != null && stateMachine.currentState == idleState || stateMachine.currentState == moveState)
            {
                stateMachine.changeState(battleState);
            }
        }
    }

    public override void Die()
    {
        if(!isDead)
        {
            stateMachine.changeState(deadState);
            base.Die();
        }
    }

    public override void Flip()
    {
        base.Flip();
    }
}
