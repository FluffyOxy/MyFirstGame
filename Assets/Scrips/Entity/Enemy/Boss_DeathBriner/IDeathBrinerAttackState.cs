using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDeathBrinerAttackState
{
    public bool CanAttack();
    public void AttackTrigger();
}
