using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic_ChestState : Mimic_StateBase
{
    public Mimic_ChestState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Mimic enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = enemy.watchCooldown;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(!enemy.isChest)
        {
            if (timer < 0)
            {
                int dice = Random.Range(0, 100);
                if (dice < enemy.watchRate)
                {
                    stateMachine.ChangeState(enemy.watchState);
                }
                timer = enemy.watchCooldown;
            }
        }
    }
}
