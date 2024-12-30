using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandAnimation: MonoBehaviour
{
    private DeathHandController_Enemy deathHand;
    private void Start()
    {
        deathHand = GetComponentInParent<DeathHandController_Enemy>();
    }

    public void AttackTrigger()
    {
        deathHand.DamageTrigger();
    }

    public void AnimFinishTrigger()
    {
        Destroy(deathHand.gameObject);
    }
}
