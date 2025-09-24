using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum ProjectPos
{
    Player,
    Target
}

[CreateAssetMenu(fileName = "New FireProjectile Effect", menuName = "Item Effect/FireProjectile")]
public class FireProjectileEffect : ItemEffect
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private ProjectPos pos;

    public override void ExcuteEffect(EffectExcuteData _targetData)
    {
        Transform projectTransform;
        switch(pos)
        {
            case ProjectPos.Player: projectTransform = PlayerManager.instance.player.transform; break;
            case ProjectPos.Target: projectTransform = _targetData.target.transform; break;
            default: projectTransform = PlayerManager.instance.player.transform; break;
        }

        Player player = PlayerManager.instance.player;
        ProjectileControllerBase newProjectile = 
            Instantiate(projectilePrefab, projectTransform.position, Quaternion.identity).GetComponent<ProjectileControllerBase>();
        newProjectile.Project(_targetData);
    }
}
