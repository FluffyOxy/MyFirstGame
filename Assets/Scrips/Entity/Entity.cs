using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    Player,
    Enemy
}
public class Entity : MonoBehaviour
{
    #region Collision
    [Header("Entity Collision Info")]
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] public Transform attackValidCheck;
    [SerializeField] public float attackValidCheckRadius;
    #endregion

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rg { get; private set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    public CharacterStats cs { get; private set; }
    #endregion

    #region Flip
    public bool isFacingLeft = false;
    public int facingDir = 1;
    #endregion

    [Header("Entity Name")]
    [SerializeField] public string entityName;

    #region Fight
    [Header("Entity Fight Info")]
    [SerializeField] protected Vector2 knockBackDir;
    [SerializeField] protected float knockBackDirAlpha = 10f;
    [SerializeField] protected float knockBackDuration = 0.07f;
    [SerializeField] public bool isKnocked;
    #endregion

    [SerializeField] float selfDestroyAfterDead = 10f;

    public bool isDead { get; protected set; }

    private float defaultGravity;

    public System.Action onFlipped;

    private bool canBeDamage_current;
    private bool canBeDamage;

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        ComponentSetUp();
        canBeDamage = true;
    }

    public bool CanBeDamage()
    {
        return canBeDamage;
    }

    public void SetCanBeDamage(bool _canBeDamage)
    {
        canBeDamage = _canBeDamage;
        canBeDamage_current = _canBeDamage;
    }

    public void SetCanBeDamage_Temp(bool _canBeDamage)
    {
        canBeDamage = _canBeDamage;
    }
    public void ResetCanBeDamage_Temp()
    {
        canBeDamage = canBeDamage_current;
    }

    protected void ComponentSetUp()
    {
        isDead = false;
        anim = GetComponentInChildren<Animator>();
        rg = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();
        sr = GetComponentInChildren<SpriteRenderer>();
        cd = GetComponent<CapsuleCollider2D>();
        cs = GetComponent<CharacterStats>();
        defaultGravity = rg.gravityScale;
    }

    protected virtual void Update()
    {
        
    }

    public virtual void slowEntityBy(float _slowPercent, float _slowDuration)
    {
        anim.speed *= (1 - _slowPercent);
    }

    protected virtual void ResetDefaultSpeed()
    {
        anim.speed = 1;
    }

    #region Collision
    public virtual void DamageAnim(Transform _damageDirection, float _damageAmount)
    {
        if(isDead)
        {
            return;
        }
        fx.StartCoroutine("FlashFX");
        StartCoroutine(HitKnockBack(_damageDirection, _damageAmount));
    }
    protected virtual IEnumerator HitKnockBack(Transform _damageDirection, float _damageAmount)
    {
        isKnocked = true;
        float knockBackFacing = 1;
        if (_damageDirection.position.x > transform.position.x)
        {
            knockBackFacing = -1;
        }
        else if(_damageDirection.position.x == transform.position.x)
        {
            knockBackFacing = -facingDir;
        }

        float alpha = _damageAmount / (_damageAmount + knockBackDirAlpha);

        SetVelocityWhenKnockBack(alpha * knockBackDir.x * knockBackFacing, alpha * knockBackDir.y);
        yield return new WaitForSeconds(knockBackDuration);
        isKnocked = false;
    }


    public virtual bool IsGrounded()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    public virtual bool IsTouchWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + facingDir * wallCheckDistance, wallCheck.position.y));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackValidCheck.position, attackValidCheckRadius);
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        isFacingLeft = !isFacingLeft;
        facingDir *= -1;
        transform.Rotate(new Vector3(0, 180, 0));

        if (onFlipped != null)
        {
            onFlipped();
        }
    }

    public void FlipCheck(float _x)
    {
        if (_x < 0 && !isFacingLeft)
        {
            Flip();
        }
        else if (_x > 0 && isFacingLeft)
        {
            Flip();
        }
    }
    #endregion

    #region Velocity
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if(isKnocked)
        {
            return;
        }
        rg.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipCheck(_xVelocity);
    }
    public void SetVelocityWithoutFlip(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
        {
            return;
        }
        rg.velocity = new Vector2(_xVelocity, _yVelocity);
    }
    public void SetVelocityWhenKnockBack(float _xVelocity, float _yVelocity)
    {
        rg.velocity = new Vector2(_xVelocity, _yVelocity);
    }
    #endregion

    public void SetGravityToDefault()
    {
        rg.gravityScale = defaultGravity;
    }

    public virtual void makeTransprent(bool _isTransprent)
    {
        if(_isTransprent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }

    public virtual void DamageSourceNotice(Entity _damageSource)
    {

    }

    //should be done at last!
    public virtual void Die()
    {
        if(!isDead)
        {
            isDead = true;
            Invoke("SelfDestroyAfterDead", selfDestroyAfterDead);
        }
    }
    private void SelfDestroyAfterDead()
    {
        if (isDead)
        {
            Vector3 viewportPosition = UnityEngine.Camera.main.WorldToViewportPoint(transform.position);
            if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
            {
                Destroy(gameObject);
            }
        }
    }

    public virtual CharacterStats GetStats()
    {
        return null;
    }
}
