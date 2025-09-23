using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class ThunderStrikeController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Entity thunderSource;
    private Entity target;
    [SerializeField] private float offset;
    [SerializeField] private float hitScale;
    private float thunderDamage;

    private Animator anim;

    private bool isHit;

    private DamageData damage = new DamageData();
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isHit)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, target.transform.position) < offset)
            {
                isHit = true;
            }
        }
        if (isHit)
        {
            anim.SetBool("Hit", true);
            transform.localScale = new Vector3(hitScale, hitScale);
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y + offset);
        }
        transform.right = target.transform.position - transform.position;
    }

    public void SetUp(Entity _targetStats, DamageData _damageDate)
    {
        isHit = false;

        target = _targetStats;

        thunderSource = _damageDate._damageSource;

        damage.SetMagicDamage(_damageDate._magical);
        damage.SetShock(
            _damageDate._shockDuration,
            _damageDate._thunderStrikeRate, 
            _damageDate._thunderStrikerCounter - 1, 
            _damageDate._attackerShockReduceAccuracy
        );
    }

    public void AnimFinishTrigger()
    {
        Destroy(gameObject);
    }

    public void AnimHitTrigger()
    {
        target.DamageSourceNotice(thunderSource);
        target.GetStats().TakeDamage(damage, transform);
        SceneAudioManager.instance.itemSFX.lightningAttack.Play(null);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + offset, transform.position.y));
    }
}
