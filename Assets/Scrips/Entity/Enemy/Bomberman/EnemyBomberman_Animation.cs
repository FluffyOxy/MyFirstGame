using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomberman_Animation : EnemyAnimation
{
    public void ExplodeTrigger()
    {
        (enemy as Enemy_Bomberman).isExplode = true;
    }

    public void ExplodeFinishTrigger()
    {
        Debug.Log("ExplodeFinishTrigger");
        enemy.stateMachine.changeState((enemy as Enemy_Bomberman).deadState);
        (enemy as Enemy_Bomberman).EnterDeadState();
    }
}
