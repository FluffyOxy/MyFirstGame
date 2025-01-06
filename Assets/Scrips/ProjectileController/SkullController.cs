using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SkullState
{
    Flying,
    Explode,
    Deflect
}

public class SkullController : MonoBehaviour, IDeflectableProjectile
{
    [SerializeField] private float damageRadius;
    [SerializeField] private float lifeDuration;
    [SerializeField] private DamageDataSerializable damageData;
    [SerializeField] private DamageDataSerializable reflectedArrowDamageData;
    [SerializeField] private float flyingSpeed;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float deflectLifeDuration;
    [SerializeField] private float deflectSpeed;

    private Entity target;
    private Entity owner;

    private Animator anim;
    private CircleCollider2D col;
    private ParticleSystem trail;

    private Vector3 deflectDir;
    private float deflectLifeTimer;
    private float lifeTimer;

    public SkullState state { get; private set; }

    public void BeDeflected()
    {
        if(state == SkullState.Flying)
        {
            deflectDir = transform.position - target.transform.position;
            deflectLifeTimer = deflectLifeDuration;
            target = owner;
            state = SkullState.Deflect;
        }
    }

    public void Setup(Entity _target, Entity _owner)
    {
        target = _target;
        owner = _owner;

        anim = GetComponent<Animator>();
        col = GetComponent<CircleCollider2D>();
        trail = GetComponentInChildren<ParticleSystem>();

        state = SkullState.Flying;
        trail.Play();
        lifeTimer = lifeDuration;
    }

    void Update()
    {
        if (state == SkullState.Flying)
        {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer < 0)
            {
                Explode();
            }
            else
            {
                transform.position =
                    Vector2.MoveTowards(transform.position, target.transform.position, flyingSpeed * Time.deltaTime);
                transform.right = target.transform.position - transform.position;
            }
        }
        else if(state == SkullState.Explode)
        {
            transform.localScale = 
                Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        else if(state == SkullState.Deflect)
        {
            deflectLifeTimer -= Time.deltaTime;
            if(deflectLifeTimer < 0)
            {
                Explode();
            }
            else
            {
                transform.position = 
                    Vector2.MoveTowards(transform.position, transform.position + deflectDir, deflectSpeed * Time.deltaTime);
                transform.right = deflectDir;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(target is Player)
        {
            if(collision.GetComponent<Player>() != null)
            {
                Explode();
            }
        }
        else if(target is Enemy)
        {
            if (collision.GetComponent<Enemy>() != null)
            {
                Explode();
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Explode();
        }
    }

    public void Explode()
    {
        if (state != SkullState.Explode)
        {
            state = SkullState.Explode;
            anim.SetTrigger("Explode");
            trail.Stop();
        }
    }

    public void DamageTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);
        foreach (Collider2D hit in colliders)
        {
            if (target is Player)
            {
                if (hit.GetComponent<Player>() != null)
                {
                    hit.GetComponent<Player>().cs.TakeDamage(damageData.GetDamageData(), transform);
                }
            }
            else if (target is Enemy)
            {
                if (hit.GetComponent<Enemy>() != null)
                {
                    hit.GetComponent<Enemy>().cs.TakeDamage(damageData.GetDamageData(), transform);
                }
            }
        }
    }
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}