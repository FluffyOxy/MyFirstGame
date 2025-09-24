using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageStroeController : MonoBehaviour
{
    private float storeTime;
    private float releaseTime;
    private float releaseCooldown;

    private bool isReleasing = false;
    private float damageStored = 0;
    private float lastHp;
    private Player player;
    private DamageData damageRelease;
    private float releaseTimer;

    public void Setup(float _storeTime, float _releaseTime, float _releaseCooldown)
    {
        player = PlayerManager.instance.player;
        lastHp = player.GetStats().GetCurrentHealthValue();
        storeTime = _storeTime;
        releaseTime = _releaseTime;
        releaseCooldown = _releaseCooldown;
    }

    private void Update()
    {
        transform.position = player.transform.position;
        if(!isReleasing)
        {
            float currentHp = player.GetStats().GetCurrentHealthValue();
            if (currentHp < lastHp)
            {
                player.GetStats().Heal(lastHp - currentHp);
                damageStored += lastHp - currentHp;
            }
            else
            {
                lastHp = currentHp;
            }

            storeTime -= Time.deltaTime;
            if(storeTime < 0)
            {
                isReleasing = true;
                int releaseCount = (int)(releaseTime / releaseCooldown);
                float releaseSpeed = damageStored / releaseCount;
                damageRelease.SetPhysicsDamage(releaseSpeed, false);
                releaseTimer = releaseCooldown;
            }
        }
        else
        {
            releaseTimer -= Time.deltaTime;
            if(releaseTimer < 0)
            {
                player.GetStats().TakeDamage(damageRelease, player.transform);
                releaseTimer = releaseCooldown;
            }

            releaseTime -= Time.deltaTime;
            if(releaseTime < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
