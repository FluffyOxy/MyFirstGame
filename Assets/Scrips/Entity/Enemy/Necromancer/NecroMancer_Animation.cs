using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroMancer_Animation : EnemyAnimation
{
    protected override void AttackTrigger()
    {
        GetComponentInParent<Enemy_Necromancer>().throwSkullToPlayer();
    }
}
