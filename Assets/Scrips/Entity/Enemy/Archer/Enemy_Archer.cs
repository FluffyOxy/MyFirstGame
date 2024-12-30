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
    public EnemyArcher_PullBackJumpState pullBackJumpState;
    public EnemyArcher_AirState airState;
    #endregion

    [Header("Pull Back Info")]
    [SerializeField] public float pullBackRadius;
    [SerializeField] public float pullBackSpeed;
    [SerializeField] public float pullBackJumpCooldown;
    [SerializeField] public Vector2 maxPullBackJumpForce;
    [SerializeField] public Vector2 minPullBackJumpForce;
    [HideInInspector] public Vector2 pullBackJumpForce;
    [SerializeField] public float pullBackJumpForceCulculateAlpha;
    [HideInInspector] public float lastPullBackJumpTime = -100;
    [SerializeField] public float edgeCheckOffset = 2f;


    [Header("Arrow")]
    [SerializeField] public GameObject arrowPrefab;
    [SerializeField] public float arrowSpeedReference;
    [SerializeField] public float arrowSpeedReference_High;
    [Range(0, 1)][SerializeField] private float speedMapK = 1;

    protected override void Awake()
    {
        base.Awake();

        attackState = new EnemyArcher_AttackState(this, stateMachine, "Attack", this);
        battleState = new EnemyArcher_BattleState(this, stateMachine, "Move", this);
        deadState = new EnemyArcher_DeadState(this, stateMachine, "Idle", this);
        idleState = new EnemyArcher_IdleState(this, stateMachine, "Idle", this);
        moveState = new EnemyArcher_MoveState(this, stateMachine, "Move", this);
        pullBackState = new EnemyArcher_PullBackState(this, stateMachine, "Move", this);
        pullBackJumpState = new EnemyArcher_PullBackJumpState(this, stateMachine, "PullBackJump", this);
        airState = new EnemyArcher_AirState(this, stateMachine, "Air", this);
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
            newArrow.Setup(EntityType.Player, arrowSpeedReference, speedMapK, this);
        }
        else
        {
            newArrow.Setup(EntityType.Player, arrowSpeedReference_High, speedMapK, this);
        }
         
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerCheck.transform.position, pullBackRadius);
    }

    public int GetPullBackDir()
    {
        Vector3 playerPosition = PlayerManager.instance.player.transform.position;

        int pullBackDir = 1;
        if (playerPosition.x > transform.position.x)
        {
            pullBackDir = -1;
        }
        return pullBackDir;
    }

    public bool TryPullBackJump()
    {
        if((Time.time - lastPullBackJumpTime) > pullBackJumpCooldown)
        {
            int pullBackDir = GetPullBackDir();

            pullBackJumpForce = maxPullBackJumpForce;
            while (true)
            {
                float jumpDuration = (2 * pullBackJumpForce.y) / (-Physics2D.gravity.y * rg.gravityScale);
                float moveDistance = pullBackJumpForce.x * jumpDuration * pullBackDir + edgeCheckOffset;

                bool haveGround = 
                    Physics2D.Raycast(
                        groundCheck.transform.position + new Vector3(pullBackDir * moveDistance, 0), 
                        Vector2.down, 
                        groundCheckDistance, 
                        whatIsGround
                    );
                bool haveWall = 
                    Physics2D.Raycast(
                        wallCheck.transform.position, 
                        Vector2.right * pullBackDir, 
                        groundCheckDistance + moveDistance, 
                        whatIsGround
                    );

                if (haveGround && !haveWall)
                {
                    stateMachine.changeState(pullBackJumpState);
                    return true;
                }
                else
                {
                    pullBackJumpForce.x -= pullBackJumpForceCulculateAlpha;
                    if (pullBackJumpForce.x < minPullBackJumpForce.x)
                    {
                        return false;
                    }
                }
            }
        }
        else
        {
            return false;
        }
    }
}