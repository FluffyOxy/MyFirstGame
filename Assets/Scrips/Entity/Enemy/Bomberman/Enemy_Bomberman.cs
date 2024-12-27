using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bomberman : Enemy
{
    #region States
    public EnemyBomberman_IdleState idleState { get; private set; }
    public EnemyBomberman_MoveState moveState { get; private set; }
    public EnemyBomberman_BattleState battleState { get; private set; }
    public EnemyBomberman_AttackState attackState { get; private set; }
    public EnemyBomberman_StunnedState stunnedState { get; private set; }
    public EnemyBomberman_DeadState deadState { get; private set; }
    public EnemyBomberman_ExplodeHoldingState explodeHoldingState { get; private set; }
    public EnemyBomberman_ExplodeStunnedState explodeStunnedState { get; private set; }
    public EnemyBomberman_ExplodeState explodeState { get; private set; }
    #endregion

    [Header("Explode Info")]
    [SerializeField] public DamageDataSerializable explodeDamage;
    [SerializeField] public DamageDataSerializable stunnedExplodeDamage;
    [SerializeField] public float holdingDuration;
    [SerializeField] public float explodeRadius;
    [SerializeField] public Transform explodeCore;
    [SerializeField] public Vector2 counterBackForce;

    [HideInInspector] public bool isExplode = false;

    protected override void Awake()
    {
        base.Awake();

        idleState = new EnemyBomberman_IdleState(this, stateMachine, "Idle", this);
        moveState = new EnemyBomberman_MoveState(this, stateMachine, "Move", this);
        battleState = new EnemyBomberman_BattleState(this, stateMachine, "Move", this);
        attackState = new EnemyBomberman_AttackState(this, stateMachine, "Attack", this);
        stunnedState = new EnemyBomberman_StunnedState(this, stateMachine, "Stunned", this);
        deadState = new EnemyBomberman_DeadState(this, stateMachine, "Idle", this);
        explodeHoldingState = new EnemyBomberman_ExplodeHoldingState(this, stateMachine, "Explode", this);
        explodeStunnedState = new EnemyBomberman_ExplodeStunnedState(this, stateMachine, "Idle", this);
        explodeState = new EnemyBomberman_ExplodeState(this, stateMachine, "Idle", this);
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
        if (stateMachine.currentState != explodeHoldingState)
        {
            if(base.TryToBeStuuned())
            {
                stateMachine.changeState(stunnedState);
                return true;
            }
            return false;
        }
        else
        {
            float counterBackDir = 1;
            if(PlayerManager.instance.player.transform.position.x > transform.position.x)
            {
                counterBackDir = -1;
            }
            rg.velocity = new Vector2(counterBackForce.x * counterBackDir, counterBackForce.y);
            stateMachine.changeState(explodeStunnedState);
            return true;
        }
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
        if (!isDead)
        {
            stateMachine.changeState(explodeHoldingState);
            base.Die();
        }
    }

    public override void Flip()
    {
        base.Flip();
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(explodeCore.position, explodeRadius);
    }
}
