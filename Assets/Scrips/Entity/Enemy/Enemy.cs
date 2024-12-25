using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Drop
{
    public ItemData item;
    [Range(0f, 100f)] public float dropChance;
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EntityFX))]
public class Enemy : Entity
{
    #region Move
    [Header("EnemyBase Move Info")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float maxIdleDuration;
    [SerializeField] public float minIdleDuration;
    private float defaultSpeed;
    #endregion

    #region PlayerDetect
    [Header("EnemyBase Player Detect")]
    [SerializeField] public Transform playerCheck;
    [SerializeField] public float playerCheckDistance;
    [SerializeField] public LayerMask whatIsPlayer;
    [SerializeField] public float playerDetectRadius;
    #endregion

    #region Attack
    [Header("EnemyBase Attack Info")]
    [SerializeField] public float toAttackDistance;
    [SerializeField] public float minAttackCooldown;
    [SerializeField] public float maxAttackCooldown;
    [HideInInspector] public float lastAttackTime;
    [SerializeField] public float battleDuration;
    [SerializeField] public float battleMoveSpeed;
    private float defaulltBattleSpeed;
    #endregion

    #region GetHit
    [Header("Enemy Stunned Info")]
    [SerializeField] public float stunDuration;
    [SerializeField] public Vector2 stunDir;
    protected bool canBeStuuned;
    [SerializeField] protected GameObject counterImage;
    #endregion

    public EnemyStats cs { get; private set; }

    [Header("Enemy Overlap Check Info")]
    [SerializeField] protected float overlapCheckRadius = 1.0f;

    [Header("Enemy Dead Animation Info")]
    [SerializeField] public float deadAnimDuration = 0.2f;
    [SerializeField] public Vector2 deadAnimVelocity = new Vector2(0, 10);

    [Header("Enemy Drop Info")]
    [SerializeField] protected List<Drop> drops;
    [SerializeField] protected GameObject itemObjectPrefab;
    [SerializeField] protected Vector2 maxDropVelocity;
    [SerializeField] protected Vector2 minDropVelocity;

    public EnemyStateMachine stateMachine;

    public string lastAnimBoolName { get; private set; }

    override protected void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
        defaultSpeed = moveSpeed;
        defaulltBattleSpeed = battleMoveSpeed;
    }

    protected override void Start()
    {
        base.Start();
        counterImage.SetActive(false);

        cs = GetComponent<EnemyStats>();
    }

    override protected void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public virtual void FreezeTime(bool _isFreeze)
    {
        if(_isFreeze)
        {
            moveSpeed = 0;
            battleMoveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultSpeed;
            anim.speed = 1;
            battleMoveSpeed = defaulltBattleSpeed;
        }
    }

    public virtual IEnumerator FreezeTimerFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStuuned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStuuned = false;
        counterImage.SetActive(false);
    }

    public bool canBeStunned()
    {
        return canBeStuuned;
    }

    public virtual bool TryToBeStuuned()
    {
        if(canBeStuuned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    public RaycastHit2D IsDetectPlayerFront()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer);
        if(hit && !hit.collider.GetComponent<Player>().isDead)
        {
            return hit;
        }
        else
        {
            return new RaycastHit2D();
        }
    }

    public bool IsPlayerNearBy()
    {
        Transform player = PlayerManager.instance.player.transform;
        return !PlayerManager.instance.player.isDead && Vector2.Distance(player.position, transform.position) < playerDetectRadius;
    }

    override public void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, playerDetectRadius);
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + playerCheckDistance * facingDir, playerCheck.position.y));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + toAttackDistance * facingDir, playerCheck.position.y));
        Gizmos.color = Color.white;
    }

    public virtual void AnimFinishTriggerCalled()
    {
        stateMachine.currentState.TriggerCall();
    }

    public bool IsEnemyOverlap()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapCheckRadius);
        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                return true;
            }
        }
        return false;
    }

    public virtual void AssignLastAnimBoolName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    public override void slowEntityBy(float _slowPercent, float _slowDuration)
    {
        base.slowEntityBy(_slowPercent, _slowDuration);
        moveSpeed *= (1 - _slowPercent);
        battleMoveSpeed *= (1 - _slowPercent);
        Invoke("ResetDefaultSpeed", _slowDuration);
    }

    protected override void ResetDefaultSpeed()
    {
        base.ResetDefaultSpeed();
        moveSpeed = defaultSpeed;
        battleMoveSpeed = defaulltBattleSpeed;
    }

    public override void Die()
    {
        if(!isDead)
        {
            DropItem();
            PlayerManager.instance.AddCurrencyAmount((int)cs.currencyDropAmount.GetValue());
            base.Die();
        }
    }
    private void DropItem()
    {
        foreach(var drop in drops)
        {
            if(Random.Range(0, 100) < drop.dropChance)
            {
                ItemObject newItemObject = Instantiate(itemObjectPrefab, transform.position, Quaternion.identity).GetComponent<ItemObject>();
                Vector2 velocity = new Vector2(Random.Range(-1, 1) * Random.Range(minDropVelocity.x, maxDropVelocity.x), Random.Range(minDropVelocity.y, maxDropVelocity.y));
                newItemObject.ThrowToward(velocity);
                newItemObject.SetItemData(drop.item);
            }
        }
    }

    public override CharacterStats GetStats()
    {
        return cs;
    }
}
