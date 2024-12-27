using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Slime : Enemy
{
    #region State
    public EnemySlime_AttackState attackState{ get; private set; }
    public EnemySlime_BattleState battleState{ get; private set; }
    public EnemySlime_DeadState deadState{ get; private set; }
    public EnemySlime_IdleState idleState{ get; private set; }
    public EnemySlime_MoveState moveState{ get; private set; }
    public EnemySlime_StunState stunnedState{ get; private set; }
    #endregion

    [Header("Self Split Info")]
    [SerializeField] private GameObject splitSlimePrefab;
    [SerializeField] private int selfSplitTime = 2;
    [SerializeField] private int selfSplitAmount = 2;
    [SerializeField] private Vector2 minSplitForce;
    [SerializeField] private Vector2 maxSplitForce;


    protected override void Awake()
    {
        base.Awake();
        StateSetup();
    }
    protected void StateSetup()
    {
        idleState = new EnemySlime_IdleState(this, stateMachine, "Idle", this);
        moveState = new EnemySlime_MoveState(this, stateMachine, "Move", this);
        battleState = new EnemySlime_BattleState(this, stateMachine, "Move", this);
        attackState = new EnemySlime_AttackState(this, stateMachine, "Attack", this);
        stunnedState = new EnemySlime_StunState(this, stateMachine, "Stunned", this);
        deadState = new EnemySlime_DeadState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        StateMachineSetup();
    }
    protected void StateMachineSetup()
    {
        stateMachine.Initialize(idleState);
        overlapCheckRadius = GetComponent<CapsuleCollider2D>().size.x;
    }

    protected override void Update()
    {
        base.Update();
    }

    public void SelfSplit()
    {
        if(selfSplitTime <= 0)
        {
            return;
        }
        for (int i = 0; i < selfSplitAmount; ++i)
        {
            Enemy_Slime newSlime = Instantiate(splitSlimePrefab, transform.position, Quaternion.identity).
                GetComponent<Enemy_Slime>();

            newSlime.isKnocked = true;
            newSlime.GetComponent<Rigidbody2D>().velocity = new Vector2(
                Random.Range(minSplitForce.x, maxSplitForce.x), 
                Random.Range(minSplitForce.y, maxSplitForce.y)
            );
            newSlime.Invoke("CancelKonckBack", 0.5f);

        }
    }
    private void CancelKonckBack() => isKnocked = false;

    public override bool TryToBeStuuned()
    {
        if (base.TryToBeStuuned())
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
        if (!isDead)
        {
            SelfSplit();
            stateMachine.changeState(deadState);
            base.Die();
        }
    }

    public override void Flip()
    {
        base.Flip();
    }
}
