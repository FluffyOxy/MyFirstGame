using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EffectFX", menuName = "Item Effect/EffectFX")]
public class EffectFX : ItemEffect
{
    [SerializeField] private GameObject effectFXPrefab;

    public override void ExcuteEffect(EffectExcuteData _targetData)
    {
        EffectFXControllerBase effectFX = 
            Instantiate(effectFXPrefab, PlayerManager.instance.player.transform.position, Quaternion.identity)
            .GetComponent<EffectFXControllerBase>();
        effectFX.PlayFX(_targetData);
    }
}
