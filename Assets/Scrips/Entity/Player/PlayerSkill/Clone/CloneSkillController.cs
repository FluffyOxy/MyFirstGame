using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CloneSkillController : MonoBehaviour
{
    private float cloneTimer;

    [SerializeField] private float colorLoosingSpeed;
    private SpriteRenderer sr;

    private Animator anim;

    [SerializeField] private Transform attackValidCheck;
    [SerializeField] private float attackValidCheckRadius;

    private Enemy closestEnemy;

    private bool canDuplicateClone;
    private float duplicateRate;
    private float duplicateSpawnOffset;
    private float facingDir = 1;

    private float cloneDamageRate;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));
        }
        if (sr.color.a < 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, Enemy _closestEnemy, bool _canDuplicateClone, float _duplicateCloneRate, float _duplicateSpawnOffset, float _cloneDamageRate)
    {
        if (_canAttack)
        {
            int randomAttackIndex = Random.Range(1, 4);
            anim.SetInteger("AttackNumber", randomAttackIndex);
            SceneAudioManager.instance.playerSFX.Attack(randomAttackIndex - 1, transform);
        }
        cloneTimer = _cloneDuration;
        transform.position = _newTransform.position + _offset;
        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicateClone;
        duplicateRate = _duplicateCloneRate;
        duplicateSpawnOffset = _duplicateSpawnOffset;
        cloneDamageRate = _cloneDamageRate;
        FaceClosestTarget();


    }

    public void AnimFinishTrigger()
    {
        cloneTimer = -1;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackValidCheck.position, attackValidCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                PlayerManager.instance.player.DoDamageTo_Clone(hit.GetComponent<Enemy>(), cloneDamageRate, transform);
                if(canDuplicateClone)
                {
                    if(Random.Range(0, 100) < duplicateRate * 100)
                    {
                        canDuplicateClone = false;
                        SkillManager.intance.clone.CreateClone(hit.transform, new Vector3(duplicateSpawnOffset * facingDir, 0));
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackValidCheck.position, attackValidCheckRadius);
    }

    private void FaceClosestTarget()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.transform.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
