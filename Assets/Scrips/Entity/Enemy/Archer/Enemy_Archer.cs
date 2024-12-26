using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class Enemy_Archer : Enemy
{
    #region Components
    public EnemyArcher_AttackState attackState;
    public EnemyArcher_BattleState battleState;
    public EnemyArcher_DeadState deadState;
    public EnemyArcher_IdleState idleState;
    public EnemyArcher_MoveState moveState;
    public EnemyArcher_PullBackState pullBackState;
    #endregion

    [Header("Pull Back Info")]
    [SerializeField] public float pullBackRadius;
    [SerializeField] public float pullBackSpeed;

    [Header("Arrow")]
    [SerializeField] public GameObject arrowPrefab;
    [SerializeField] public float arrowSpeedReference;
    [SerializeField] public float arrowSpeedReference_High;

    protected override void Awake()
    {
        base.Awake();

        attackState = new EnemyArcher_AttackState(this, stateMachine, "Attack", this);
        battleState = new EnemyArcher_BattleState(this, stateMachine, "Move", this);
        deadState = new EnemyArcher_DeadState(this, stateMachine, "Idle", this);
        idleState = new EnemyArcher_IdleState(this, stateMachine, "Idle", this);
        moveState = new EnemyArcher_MoveState(this, stateMachine, "Move", this);
        pullBackState = new EnemyArcher_PullBackState(this, stateMachine, "Move", this);
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
        return false;
    }

    public override void DamageSourceNotice(Entity _damageSource)
    {
        if (_damageSource != null)
        {
            Player player = _damageSource as Player;
            if (player != null)
            {
                stateMachine.changeState(battleState);
            }
        }
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

    public bool shouldPullBack()
    {
        Transform player = PlayerManager.instance.player.transform;
        return !PlayerManager.instance.player.isDead && Vector2.Distance(player.position, transform.position) < pullBackRadius;
    }

    public void ShootAnArrowToPlayer()
    {
        ArrowController newArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity).GetComponent<ArrowController>();
        if(CanSeePlayer())
        {
            newArrow.Setup(EntityType.Player, arrowSpeedReference);
        }
        else
        {
            newArrow.Setup(EntityType.Player, arrowSpeedReference_High);
        }
         
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerCheck.transform.position, pullBackRadius);
    }
}