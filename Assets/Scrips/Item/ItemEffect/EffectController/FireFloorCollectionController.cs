using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//¸ºÔð·¢ÉäFireFloor
public class FireFloorCollectionController : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private float fireCooldown;
    [SerializeField] private GameObject fireFloorPrefab;

    private float fireTimer;
    private EffectExcuteData targetData;

    public void Setup(EffectExcuteData _targetData)
    {
        fireTimer = fireCooldown;
        targetData = _targetData;
    }

    void Update()
    {
        transform.position = PlayerManager.instance.player.transform.position;
        lifeTime -= Time.deltaTime;
        fireTimer -= Time.deltaTime;
        if(lifeTime < 0)
        {
            Destroy(gameObject);
        }
        if(fireTimer < 0)
        {
            fireTimer = fireCooldown;
            FireFloorEffectController fireFloor =
            Instantiate(fireFloorPrefab, PlayerManager.instance.player.transform.position, Quaternion.identity)
            .GetComponentInChildren<FireFloorEffectController>();
            fireFloor.Project(targetData);
        }
    }
}
