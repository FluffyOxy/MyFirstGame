using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D cd;

    private float crystalTimer;
    private bool canExplode;
    private bool canGrow;
    private float growSpeed;
    private float maxSize;
    private bool canMove;
    private float moveSpeed;

    private Enemy targetEnemy;

    private float smallDistance_CrystalExpolde;
    private float smallDistance_CrystalCreateClone;

    private bool isCrytalClone;

    public void SetUp(float _crystalDuration, bool _canExplode, float _growSpeed, float _maxSize, Enemy _targetEnemy, bool _canMove, float _moveSpeed, bool _isCrystalClone)
    {
        Debug.Log("Crystal SetUp");

        crystalTimer = _crystalDuration;
        canExplode = _canExplode && !SkillManager.intance.crystal.CanCreateClone_CrystalSwap();
        growSpeed = _growSpeed;
        maxSize = _maxSize;
        targetEnemy = _targetEnemy;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        isCrytalClone = _isCrystalClone;

        anim = GetComponent<Animator>();
        cd = GetComponent<CircleCollider2D>();

        if(!canMove)
        {
            SceneAudioManager.instance.playerSFX.crystalPlace.Play(null);
        }
    }

    public void SetUpTestInfo(float _smallDistance_CrystalExpolde, float _smallDistance_CrystalCreateClone)
    {
        smallDistance_CrystalExpolde = _smallDistance_CrystalExpolde;
        smallDistance_CrystalCreateClone = _smallDistance_CrystalCreateClone;
    }

    private void Update()
    {
        crystalTimer -= Time.deltaTime;
        if (crystalTimer < 0)
        {
            FinishCrystal();
        }
        if(canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if (canMove && targetEnemy != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetEnemy.transform.position, moveSpeed * Time.deltaTime);
            transform.up = targetEnemy.transform.position - transform.position;

            float smallDistance = 0f;
            if(SkillManager.intance.crystal.CanCreateClone_CrystalSwap())
            {
                smallDistance = smallDistance_CrystalCreateClone;
            }
            else if(canExplode)
            {
                smallDistance = smallDistance_CrystalExpolde;
            }

            if(Vector2.Distance(transform.position, targetEnemy.transform.position) < smallDistance)
            {
                FinishCrystal();
            }
        }
    }

    public void FinishCrystal()
    {
        if(SkillManager.intance.crystal.TryCreateClone_CrystalSwap(transform))
        {
            SelfDestroy();
            return;
        }
        if (canExplode)
        {
            canGrow = true;
            anim.SetBool("Explode", true);
            canMove = false;

            SceneAudioManager.instance.playerSFX.crystalExplode.Play(transform);
        }
        else
        {
            SelfDestroy();
        }
    }

    public void SelfDestroy()
    {
        if (isCrytalClone && !SkillManager.intance.clone.isUnlocked_crystalInsteadOfClone)
        {
            SkillManager.intance.clone.CreateClone(transform, Vector3.zero);
        }
        if(!SkillManager.intance.crystal.isUnlocked_multiStacks)
        {
            SkillManager.intance.crystal.SetCoolDown();
        }
        Destroy(gameObject);
    }

    public void ExplodeAnimTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius * maxSize);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                PlayerManager.instance.player.DoDamageTo_Crystal(hit.GetComponent<Enemy>(), transform);
            }
        }
    }
}
