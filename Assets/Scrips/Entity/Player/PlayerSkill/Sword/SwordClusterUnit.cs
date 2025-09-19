using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordClusterUnit : MonoBehaviour
{
    private float damageRate;
    private float fadeOutTime;

    private List<SwordClusterUnit> nearUnits;
    private List<SwordClusterUnit> collisionUnits;
    private SwordClusterSpawner spawner;

    private Vector2 currentVelocity;
    private Vector2 nextVelocity;

    private bool isStuck = false;
    private bool isEnding = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (!isStuck && !isEnding)
        {
            UpdateNearAndCollisionUnits();

            Vector2 velocityMatchingVector = GetVelocityMatchingVector();
            Vector2 centerTargetingVector = GetCenterTargetingVector();
            Vector2 collisionPreventingVector = GetCollisionPreventingVector();
            Vector2 mouseTargetingVector = GetMouseTargetingVector();

            nextVelocity = velocityMatchingVector * spawner.currentSwordInfo.velocityMatchingArg +
                           centerTargetingVector * spawner.currentSwordInfo.centerTargetingArg +
                           collisionPreventingVector * spawner.currentSwordInfo.collisionPreventingArg +
                           mouseTargetingVector * spawner.currentSwordInfo.mouseTargetingArg;
            nextVelocity *= spawner.currentSwordInfo.scaleArg;
        }
    }

    private void OnTriggerEnter2D(Collider2D _hit)
    {
        StuckInto(_hit.transform);
        PlayerManager.instance.player.DoDamageTo_Sword(_hit.GetComponent<Enemy>(), damageRate, transform);
    }

    private void StuckInto(Transform _target)
    {
        currentVelocity.x = 0;
        currentVelocity.y = 0;
        nextVelocity = currentVelocity;
        GetComponent<CapsuleCollider2D>().enabled = false;
        transform.parent = _target;
        isStuck = true;
        SceneAudioManager.instance.playerSFX.swordGround.Play(transform);
        GetComponentInChildren<ParticleSystem>().Play();
    }

    void LateUpdate()
    {
        if (!isStuck)
        {
            currentVelocity = Vector2.Lerp(currentVelocity, nextVelocity, spawner.currentSwordInfo.velocityLerpArg);

            if (currentVelocity.magnitude < spawner.currentSwordInfo.minVelocity)
            {
                currentVelocity = currentVelocity.normalized * spawner.currentSwordInfo.minVelocity;
            }
            else if (currentVelocity.magnitude > spawner.currentSwordInfo.maxVelocity)
            {
                currentVelocity = currentVelocity.normalized * spawner.currentSwordInfo.maxVelocity;
            }

            if (currentVelocity.magnitude != 0)
            {
                transform.right = currentVelocity;
            }
            transform.position += (Vector3)currentVelocity * Time.deltaTime;
        }
    }

    public void Setup(SwordClusterSpawner _spawner, float _damageRate, float _fadeOutTime)
    {
        currentVelocity = new Vector2(0, 0);
        nextVelocity = new Vector2(0, 0);
        nearUnits = new List<SwordClusterUnit>();
        collisionUnits = new List<SwordClusterUnit>();
        spawner = _spawner;
        damageRate = _damageRate;
        fadeOutTime = _fadeOutTime;
    }

    public void SelfDestory_FadeOut()
    {
        isEnding = true;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        float fadeOutSmooth = 0.05f;
        float fadeOutFreq = fadeOutTime * fadeOutSmooth;
        while (sr.color.a > 0)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - fadeOutSmooth);
            yield return new WaitForSeconds(fadeOutFreq);
        }
        Destroy(gameObject);
    }

    private void UpdateNearAndCollisionUnits()
    {
        float dist;
        float nearestUnitDist = float.MaxValue;
        SwordClusterUnit nearestUnit = this;

        nearUnits.Clear();
        collisionUnits.Clear();

        foreach (var unit in spawner.swordUnits)
        {
            if(unit == this)
                continue;

            dist = Vector2.Distance(transform.position, unit.transform.position);

            if (dist < spawner.currentSwordInfo.nearRadius)
            {
                nearUnits.Add(unit);
            }

            if (dist < spawner.currentSwordInfo.collisionRadius)
            {
                collisionUnits.Add(unit);
            }

            if (dist < nearestUnitDist)
            {
                nearestUnitDist = dist;
                nearestUnit = unit;
            }
        }

        if (nearUnits.Count == 0 && nearestUnit != this)
        {
            nearUnits.Add(nearestUnit);
        }

        
    }

    private Vector2 GetVelocityMatchingVector()
    {
        if (nearUnits.Count == 0)
        {
            return new Vector2(0, 0);
        }

        Vector2 sumVelocity = new Vector2();
        foreach (var unit in nearUnits)
        {
            sumVelocity += unit.currentVelocity;
        }

        return sumVelocity / nearUnits.Count;
    }

    private Vector2 GetCenterTargetingVector()
    {
        if (nearUnits.Count == 0)
        {
            return new Vector2(0, 0);
        }

        Vector2 nearUnitsAvgPos = GetAvgPosition(nearUnits);

        return nearUnitsAvgPos - (Vector2)transform.position;
    }
    private Vector2 GetCollisionPreventingVector()
    {
        if (collisionUnits.Count == 0)
        {
            return new Vector2(0, 0);
        }

        Vector2 nearUnitsAvgPos = GetAvgPosition(collisionUnits);

        return (Vector2)transform.position - nearUnitsAvgPos;
    }

    private Vector2 GetAvgPosition(List<SwordClusterUnit> _units)
    {
        Vector2 sumPosition = new Vector2();
        foreach (var unit in nearUnits)
        {
            sumPosition += (Vector2)unit.transform.position;
        }

        return sumPosition / nearUnits.Count;
    }

    private Vector2 GetMouseTargetingVector()
    {
        return spawner.mousePos - (Vector2)transform.position;
    }

}
