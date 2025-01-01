using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


[Serializable]
public class DeathBrinerStageInfo
{
    [Header("AttackPlan Info")]
    [SerializeField] public int maxAttackAmount;
    [SerializeField] public int minAttackAmount;

    [Header("Flash Attack Info")]
    [Range(0, 100)][SerializeField] public float flashAttackRate_remote;
    [Range(0, 100)][SerializeField] public float flashAttackRate_primary;
    [SerializeField] public List<Transform> flashRemoteAttackPosition;

    [Header("Dash Attack Info")]
    [Range(0, 100)][SerializeField] public float dashAttackRate;
    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashAnimPlaySpeed;
    [SerializeField] public float dashDamageCooldown;

    [Header("Shadow Attack Info")]
    [Range(0, 100)][SerializeField] public float shadowAttackRate;
    [SerializeField] public int minShadowAttackAmount;
    [SerializeField] public int maxShadowAttackAmount;
    [SerializeField] public float shadowMoveSpeed;
    [SerializeField] public float shadowDamageCooldown;
    [SerializeField] public float minShadowIdleDuration;
    [SerializeField] public float maxShadowIdleDuration;
}

public class EnemyBoss_DeathBriner : Enemy, IStageEntity
{
    #region States
    public DeathBriner_IdleState idleState;
    public DeathBriner_PrimaryAttackState primaryAttackState;
    public DeathBriner_RunMoveState runMoveState;
    public DeathBriner_JumpState jumpState;
    public DeathBriner_RemoteAttackState remoteAttackState;
    public DeathBriner_FlashMoveState flashMoveState;
    public DeathBriner_StunState stunState;
    public DeathBriner_DashAttackState dashAttackState;
    public DeathBriner_ShadowAttack shadowAttackState;
    public DeathBriner_DeadState deadState;
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
    [SerializeField] public List<Transform> flashRemoteAttackPosition;
    [HideInInspector] public bool isFlashOut;

    [Header("Dash Attack Info")]
    [Range(0, 100)][SerializeField] public float dashAttackRate;
    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashAnimPlaySpeed;
    [SerializeField] public float dashDamageCooldown;
    [SerializeField] public bool isDashing;

    [Header("Shadow Attack Info")]
    [Range(0, 100)][SerializeField] public float shadowAttackRate;
    [SerializeField] public int minShadowAttackAmount;
    [SerializeField] public int maxShadowAttackAmount;
    [SerializeField] public float shadowMoveSpeed;
    [SerializeField] public ParticleSystem shadowParticle;
    [SerializeField] public float shadowDamageRadius;
    [SerializeField] public float shadowDamageCooldown;
    [SerializeField] public float minShadowIdleDuration;
    [SerializeField] public float maxShadowIdleDuration;
    [SerializeField] public float shadowIdleMoveUpSpeed = 5f;

    [Header("To Be Stunned Info")]
    [SerializeField] private CircleCollider2D primaryAttackCollider;
    [SerializeField] private CircleCollider2D shadowAttackCollider;

    [Header("Stage Info")]
    [SerializeField] private List<StageController> stages;
    [SerializeField] private List<DeathBrinerStageInfo> deathBrinerStageInfos;
    private int liveCounter = 0;
    public bool isStageChanging = false;

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
        dashAttackState = new DeathBriner_DashAttackState(this, stateMachine, "Dash", this);
        shadowAttackState = new DeathBriner_ShadowAttack(this, stateMachine, "Flash", this);
        deadState = new DeathBriner_DeadState(this, stateMachine, "Idle", this);
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        overlapCheckRadius = GetComponent<CapsuleCollider2D>().size.x;
        primaryAttackCollider.enabled = true;
        shadowAttackCollider.enabled = false;
    }

    protected override void Update()
    {
        base.Update();
    }

    public void SetShadowStunCollider(bool _isShadow)
    {
        if(_isShadow)
        {
            primaryAttackCollider.enabled = false;
            shadowAttackCollider.enabled = true;
        }
        else
        {
            primaryAttackCollider.enabled = true;
            shadowAttackCollider.enabled = false;
        }
    }

    public override bool TryToBeStuuned()
    {
        if (base.TryToBeStuuned())
        {
            if(stateMachine.currentState == shadowAttackState)
            {
                shadowAttackState.BeStunned();
            }
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
        stages[liveCounter].StageExit();

        if (liveCounter >= stages.Count - 1)
        {
            if (!isDead)
            {
                stateMachine.changeState(deadState);
                base.Die();
            }
        }
        else
        {
            ++liveCounter;

            isStageChanging = true;

            LoadStageInfo(deathBrinerStageInfos[liveCounter]);
            cs.SetCurrentHealth(cs.GetStatByType(StatType.MaxHealth));

            currentAttackState = shadowAttackState;
            shadowAttackState.isEscape = true;
            stateMachine.changeState(shadowAttackState);
        }
    }
    private void LoadStageInfo(DeathBrinerStageInfo _info)
    {
        minAttackAmount = _info.minAttackAmount;
        maxAttackAmount = _info.maxAttackAmount;

        flashAttackRate_remote = _info.flashAttackRate_remote;
        flashAttackRate_primary = _info.flashAttackRate_primary;
        flashRemoteAttackPosition = _info.flashRemoteAttackPosition;

        dashAttackRate = _info.dashAttackRate;
        dashSpeed = _info.dashSpeed;
        dashAnimPlaySpeed = _info.dashAnimPlaySpeed;
        dashDamageCooldown = _info.dashDamageCooldown;

        shadowAttackRate = _info.shadowAttackRate;
        minShadowAttackAmount = _info.minShadowAttackAmount;
        maxShadowAttackAmount = _info.maxShadowAttackAmount;
        shadowMoveSpeed = _info.shadowMoveSpeed;
        minShadowIdleDuration = _info.minShadowIdleDuration;
        maxShadowIdleDuration = _info.maxShadowIdleDuration;
    }
    public void StageChangeFinish()
    {
        isStageChanging = false;
    }

    public override void Flip()
    {
        base.Flip();
    }

    public void SetRandomAttackCount()
    {
        attackCounter = UnityEngine.Random.Range(minAttackAmount, maxAttackAmount);
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(shadowParticle.transform.position, shadowDamageRadius);
    }

    public void SetRandomIdleDuration()
    {
        idleDuration = UnityEngine.Random.Range(minIdleDuration, maxIdleDuration);
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
