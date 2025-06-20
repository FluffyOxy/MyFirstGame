using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ClusterInfo
{
    public float nearRadius;
    public float collisionRadius;
    public float minVelocity;
    public float maxVelocity;
    public float velocityMatchingArg;
    public float centerTargetingArg;
    public float collisionPreventingArg;
    public float mouseTargetingArg;
    public float velocityLerpArg;
    public float scaleArg;
}

public class SwordClusterSpawner : MonoBehaviour
{
    [Header("SwordSpawnInfo")]
    public int swordCount;
    public float swordLaunchCooldown;
    public float swordFadeOutTime;
    public float swordLifetime;
    public float spawnRadius;
    public GameObject swordUnitPrefab;
    public List<SwordClusterUnit> swordUnits;

    [Header("SwordClusterMovementInfo")] 
    public ClusterInfo launchingSwordInfo;
    public ClusterInfo flyingSwordInfo;

    [SerializeField] private float launchingDuration = 1f;

    [HideInInspector] public ClusterInfo currentSwordInfo;

    public Vector2 mousePos;


    private void Start()
    {
        currentSwordInfo = launchingSwordInfo;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void Launch(float _damageRate)
    {
        currentSwordInfo = launchingSwordInfo;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        StartCoroutine(LaunchSwords(_damageRate));
        StartCoroutine(SwordClusterLifeControl());
    }

    private IEnumerator LaunchSwords(float _damageRate)
    {
        for (int index = 0; index < swordCount; ++index)
        {
            float xOffset = Random.Range(-1f, 1f);
            float yOffset = Mathf.Sqrt(1 - Mathf.Pow(xOffset, 2));
            float rOffset = Random.Range(0, spawnRadius);

            GameObject swordUnitGameObject = Instantiate(
                swordUnitPrefab,
                new Vector2(transform.position.x + xOffset * rOffset, transform.position.y + yOffset * rOffset),
                Quaternion.identity
            );
            SwordClusterUnit swordClusterUnit = swordUnitGameObject.GetComponent<SwordClusterUnit>();
            swordClusterUnit.Setup(this, _damageRate, swordFadeOutTime);
            swordUnits.Add(swordClusterUnit);

            yield return new WaitForSeconds(swordLaunchCooldown);
        }

        LaunchingFinish();
    }
    private void LaunchingFinish()
    {
        currentSwordInfo = flyingSwordInfo;
    }

    private IEnumerator SwordClusterLifeControl()
    {
        yield return new WaitForSeconds(swordLifetime);
        foreach (var unit in swordUnits)
        {
            unit.SelfDestory_FadeOut();
        }
        swordUnits.Clear();
        Destroy(gameObject);
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mouseImage.transform.position = mousePos;
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
