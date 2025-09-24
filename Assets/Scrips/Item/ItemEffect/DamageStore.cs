using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DamageStore Effect", menuName = "Item Effect/DamageStore")]
public class DamageStore : ItemEffect
{
    [SerializeField] private float storeTime;
    [SerializeField] private float releaseTime;
    [SerializeField] private float releaseCooldown = 1;

    [SerializeField] private GameObject controllerPrefab;

    public override void ExcuteEffect(EffectExcuteData _targetData)
    {
        DamageStroeController controller = 
            Instantiate(controllerPrefab, PlayerManager.instance.player.transform.position, Quaternion.identity)
            .GetComponent<DamageStroeController>();
        controller.Setup(storeTime, releaseTime, releaseCooldown);
    }
}
