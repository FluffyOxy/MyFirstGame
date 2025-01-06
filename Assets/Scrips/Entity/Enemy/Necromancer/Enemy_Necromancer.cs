using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Necromancer : Enemy
{
    #region Components
    public Necromancer_AttackState attackState;
    public Necromancer_BattleState battleState;
    public Necromancer_ControllState controllingState;
    public Necromancer_DeadState deadState;
    public Necromancer_IdleState idleState;
    public Necromancer_MoveState moveState;
    #endregion

    [Header("Move Info")]
    [SerializeField] public float flyingHeight;
    [SerializeField] public float verticalFlyingSpeed;

    [Header("Battle Info")]
    [SerializeField] public float pullbackRadius;
    [SerializeField] private GameObject skullPrefab;
    [SerializeField] private Transform skullThrowPosition;

    [HideInInspector] public bool isAnimSkullThrowTrigger;
    [HideInInspector] public SkullController currentSkull;
    [HideInInspector] public bool isTakingDamage;


    protected override void Awake()
    {
        base.Awake();

        attackState = new Necromancer_AttackState(this, stateMachine, "Attack", this);
        battleState = new Necromancer_BattleState(this, stateMachine, "Move", this);
        deadState = new Necromancer_DeadState(this, stateMachine, "Idle", this);
        idleState = new Necromancer_IdleState(this, stateMachine, "Idle", this);
        moveState = new Necromancer_MoveState(this, stateMachine, "Move", this);
        controllingState = new Necromancer_ControllState(this, stateMachine, "Controll", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        overlapCheckRadius = GetComponent<CapsuleCollider2D>().size.x;
        isTakingDamage = false;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool TryToBeStuuned()
    {
        return false;
    }

    public override void DamageSourceNotice(Entity _damageSource)
    {
        isTakingDamage = true;
        Invoke("CancelTakingDamage", 3 * Time.deltaTime);

        if (_damageSource != null)
        {
            Player player = _damageSource as Player;
            if (player != null && stateMachine.currentState == idleState || stateMachine.currentState == moveState)
            {
                stateMachine.changeState(battleState);
            }
        }
    }
    private void CancelTakingDamage()
    {
        isTakingDamage = false;
    }

    public override void Die()
    {
        if (!isDead)
        {
            stateMachine.changeState(deadState);
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
        Gizmos.DrawWireSphere(transform.position, pullbackRadius);
    }

    public override bool CanAttack()
    {
        if(CanSeePlayer())
        {
            return base.CanAttack();
        }
        return false;
    }

    public bool shouldPullBack(bool _isCurrentPullback)
    {
        Transform player = PlayerManager.instance.player.transform;
        if(!_isCurrentPullback)
        {
            return !PlayerManager.instance.player.isDead && Vector2.Distance(player.position, transform.position) < pullbackRadius;
        }
        else
        {
            return !PlayerManager.instance.player.isDead && Vector2.Distance(player.position, transform.position) < toAttackRadius;
        }
    }

    public void throwSkullToPlayer()
    {
        currentSkull = Instantiate(skullPrefab, skullThrowPosition.position, Quaternion.identity).GetComponent<SkullController>();
        currentSkull.Setup(PlayerManager.instance.player, this);
    }
}