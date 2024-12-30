using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class EnemyBoss_DeathBriner : Enemy
{
    #region States
    public DeathBriner_IdleState idleState;
    public DeathBriner_PrimaryAttackState primaryAttackState;
    public DeathBriner_RunMoveState runMoveState;
    public DeathBriner_JumpState jumpState;
    public DeathBriner_RemoteAttackState remoteAttackState;
    public DeathBriner_FlashMoveState flashMoveState;
    public DeathBriner_StunState stunState;
    #endregion

    [Header("AttackPlan Info")]
    [SerializeField] public int maxAttackAmount;
    [SerializeField] public int minAttackAmount;
    [HideInInspector] public int attackCounter;
    [HideInInspector] public IDeathBrinerAttackState currentAttackState;

    [Header("Jump Info")]
    [SerializeField] public float pullBackJumpCooldown;
    [SerializeField] public float pullBackJumpRadius;
    [SerializeField] public float edgeCheckOffset = 2f;
    [SerializeField] public Vector2 pullBackJumpForce;
    [HideInInspector] public float lastPullBackJumpTime = -100;
    [HideInInspector] public int jumpDir;

    [Header("Remote Attack Info")]
    [SerializeField] public GameObject deathHandPrefab;

    [Header("Move Info")]
    [SerializeField] public float minMoveDuration;
    [SerializeField] public float maxMoveDuration;

    [Header("Flash Attack Info")]
    [Range(0, 100)][SerializeField] public float flashAttackRate_remote;
    [Range(0, 100)][SerializeField] public float flashAttackRate_primary;
    [SerializeField] public List<Transform> flashRemoteAttackPosition_1;
    [SerializeField] public List<Transform> flashRemoteAttackPosition_2;
    [HideInInspector] public bool isFlashOut;

    [HideInInspector] public float idleDuration;
    [HideInInspector] public bool isPullBack;
    [HideInInspector] public bool isTakingDamage;

    protected override void Awake()
    {
        base.Awake();

        idleState = new DeathBriner_IdleState(this, stateMachine, "Idle", this);
        primaryAttackState = new DeathBriner_PrimaryAttackState(this, stateMachine, "PrimaryAttack", this);
        runMoveState = new DeathBriner_RunMoveState(this, stateMachine, "RunMove", this);
        jumpState = new DeathBriner_JumpState(this, stateMachine, "Jump", this);
        remoteAttackState = new DeathBriner_RemoteAttackState(this, stateMachine, "RemoteAttack", this);
        flashMoveState = new DeathBriner_FlashMoveState(this, stateMachine, "Flash", this);
        stunState = new DeathBriner_StunState(this, stateMachine, "Stun", this);
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
        if (base.TryToBeStuuned())
        {
            stateMachine.changeState(stunState);
            return true;
        }
        return false;
    }

    public override void DamageSourceNotice(Entity _damageSource)
    {
        isTakingDamage = true;
        Invoke("CancelTakingDamage", 2 * Time.deltaTime);
    }
    private void CancelTakingDamage()
    {
        isTakingDamage = false;
    }

    public override void Die()
    {
        if (!isDead)
        {
            
            base.Die();
        }
    }

    public override void Flip()
    {
        base.Flip();
    }

    public void SetRandomAttackCount()
    {
        attackCounter = Random.Range(minAttackAmount, maxAttackAmount);
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

    public bool CanJump()
    {
        return (Time.time - lastPullBackJumpTime) > pullBackJumpCooldown;
    }
    public bool ShouldPullBackJump()
    {
        return Mathf.Abs(transform.position.x - PlayerManager.instance.player.transform.position.x) < pullBackJumpRadius;
    }
    public void CalculatePullBackJumpParameter()
    {
        float moveDistance = toAttackRadius - Mathf.Abs(transform.position.x - PlayerManager.instance.player.transform.position.x);

        float jumpDuration = moveDistance / pullBackJumpForce.x;
        pullBackJumpForce.y = (-Physics2D.gravity.y * rg.gravityScale * jumpDuration) / 2f;

        moveDistance += edgeCheckOffset;

        jumpDir = GetPullBackDir();
        if (!IsMoveSafe(jumpDir, moveDistance))
        {
            jumpDir = -jumpDir;
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pullBackJumpRadius);
    }

    public void SetRandomIdleDuration()
    {
        idleDuration = Random.Range(minIdleDuration, maxIdleDuration);
    }

    public void GenerateDeathHand()
    {
        DeathHandController_Enemy newDeathHand = 
            Instantiate(deathHandPrefab, PlayerManager.instance.player.transform.position, Quaternion.identity)
            .GetComponent<DeathHandController_Enemy>();
        newDeathHand.Setup(this);
    }

    public bool IsMoveSafe(int _moveDir, float _moveDistance)
    {
        bool haveGround =
            Physics2D.Raycast(
                groundCheck.transform.position + new Vector3(_moveDir * _moveDistance, 0),
                Vector2.down,
                groundCheckDistance,
                whatIsGround
            );
        bool haveWall =
            Physics2D.Raycast(
                wallCheck.transform.position,
                Vector2.right * _moveDir,
                groundCheckDistance + _moveDistance,
                whatIsGround
            );
        return haveGround && !haveWall;
    }


}
