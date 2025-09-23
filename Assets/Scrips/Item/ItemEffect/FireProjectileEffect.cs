using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New FireProjectile Effect", menuName = "Item Effect/FireProjectile")]
public class FireProjectileEffect : ItemEffect
{
    [SerializeField] private GameObject projectilePrefab;

    public override void ExcuteEffect(EffectExcuteData _targetData)
    {
        Player player = PlayerManager.instance.player;
        ProjectileControllerBase newProjectile = 
            Instantiate(projectilePrefab, _targetData.target.transform.position, Quaternion.identity).GetComponent<ProjectileControllerBase>();
        newProjectile.Project(_targetData);
    }
}
