using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("Base Skill Info")]
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;
    [SerializeField] private string cooldownWarningText = "ÀäÈ´ÖÐ";

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool IsOutCooldown()
    {
        if( cooldownTimer > 0 )
        {
            player.fx.CreatePopUpText(cooldownWarningText);
        }
        return cooldownTimer < 0;
    }
    public virtual void SetCoolDown()
    {
        cooldownTimer = cooldown;
    }

    public virtual bool TryUseSkill()
    {
        if(cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        else
        {
            player.fx.CreatePopUpText(cooldownWarningText);
            return false;
        }
    }

    public virtual void UseSkill()
    {

    }

    protected virtual Enemy TryFindClosestEnemy(Transform _checkTransform, float _radius = 20.0f)
    {
        Enemy closestEnemy = null;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, _radius);

        float minDistance = Mathf.Infinity;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distance = Vector2.Distance(_checkTransform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = hit.GetComponent<Enemy>();
                }
            }
        }

        return closestEnemy;
    }

    public virtual float GetCooldownPercentage()
    {
        if(cooldownTimer >= 0)
        {
            return cooldownTimer / cooldown;
        }
        else
        {
            return 0;
        }
    }

    public virtual float GetCooldownRestSecond()
    {
        if(cooldownTimer >= 0)
        {
            return cooldownTimer;
        }
        else
        {
            return 0;
        }
    }
}
