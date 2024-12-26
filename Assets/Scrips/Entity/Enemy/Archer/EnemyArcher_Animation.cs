using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher_Animation : EnemyAnimation
{
    protected override void AttackTrigger()
    {
        if (PlayerManager.instance.player.transform.position.x < enemy.transform.position.x && !enemy.isFacingLeft)
        {
            enemy.Flip();
        }
        else if (PlayerManager.instance.player.transform.position.x > enemy.transform.position.x && enemy.isFacingLeft)
        {
            enemy.Flip();
        }
        GetComponentInParent<Enemy_Archer>().ShootAnArrowToPlayer();
    }
}