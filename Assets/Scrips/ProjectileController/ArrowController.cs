using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour, IDeflectableProjectile
{
    CapsuleCollider2D col;
    Rigidbody2D rg;
    SpriteRenderer sr;
    ParticleSystem trail;

    [SerializeField] private DamageDataSerializable damageData;
    [SerializeField] private DamageDataSerializable reflectedArrowDamageData;
    [SerializeField] private float liveDuration = 2.0f;
    [SerializeField] private float fadeSpeed = 1.0f;
    private bool isStuck;
    private float liveTimer;
    [SerializeField] private EntityType targetType;


    private void Start()
    {
        
    }

    private void Update()
    {
        if(isStuck)
        {
            liveTimer -= Time.deltaTime;
            if (liveTimer <= 0)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - fadeSpeed * Time.deltaTime);
                if(sr.color.a <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            transform.right = rg.velocity;
        }
    }

    public void Setup(EntityType _targetType, float _arrowSpeedReference, float _speedMapK, Entity _owner)
    {
        col = GetComponent<CapsuleCollider2D>();
        rg = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        trail = GetComponentInChildren<ParticleSystem>();

        liveTimer = liveDuration;
        isStuck = false;
        targetType = _targetType;

        rg.velocity = CulculateArrowSpeed(PlayerManager.instance.player, _arrowSpeedReference, _speedMapK);
        damageData.SetDamageSource(_owner);
    }
    private Vector2 CulculateArrowSpeed(Entity _target, float _arrowSpeedReference, float _speedMapK)
    {
        float distanceToPlayer = Vector2.Distance(_target.transform.position, transform.position);
        float timeToHit = distanceToPlayer / _arrowSpeedReference;

        Vector2 sourcePosition = transform.position;

        float affectFactor = 1 - 1 / Mathf.Pow(1 + _arrowSpeedReference, _speedMapK);
        Vector2 targetPositionAfterTimeToHit = (Vector2)_target.transform.position + _target.rg.velocity * timeToHit * affectFactor;
        if(!_target.IsGrounded())
        {
            targetPositionAfterTimeToHit += 0.5f * Physics2D.gravity * _target.rg.gravityScale * timeToHit * timeToHit * affectFactor;
        }

        Vector2 arrowVelocity = (
                    targetPositionAfterTimeToHit -
                    sourcePosition -
                    0.5f * (Physics2D.gravity * rg.gravityScale) * (timeToHit * timeToHit)
                ) / timeToHit;

        return arrowVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if(targetType == EntityType.Player)
            {
                collision.GetComponent<Player>().cs.TakeDamage(damageData.GetDamageData(), transform);
                StuckInto(collision);
            }
        }
        else if (collision.GetComponent<Enemy>() != null)
        {
            if (targetType == EntityType.Enemy)
            {
                collision.GetComponent<Enemy>().cs.TakeDamage(damageData.GetDamageData(), transform);
                StuckInto(collision);
            }
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StuckInto(collision);
        }
    }
    private void StuckInto(Collider2D collision)
    {
        col.enabled = false;
        rg.isKinematic = true;
        rg.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;
        trail.Stop();
        trail.Clear();
        isStuck = true;
    }

    public void BeDeflected()
    {
        if(targetType == EntityType.Player)
        {
            rg.velocity = -rg.velocity;
            targetType = EntityType.Enemy;
            damageData = reflectedArrowDamageData;
        }
    }
}
