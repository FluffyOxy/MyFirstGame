using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhost_FlyState : EnemyGhost_WanderState
{
    private const int DESTINATION_SEARCHING_TIME = 5;

    private int currentDestinationSearchingTime;

    public EnemyGhost_FlyState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName,
        EnemyGhost _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        currentDestinationSearchingTime = DESTINATION_SEARCHING_TIME;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (!enemy.IsDestinationValid())
        {
            if (currentDestinationSearchingTime < 0)
            {
                stateMachine.changeState(enemy.idleState);
            }
            else
            {
                --currentDestinationSearchingTime;
                Vector3 newDestination = GetRandomDestination();
                enemy.SetDestination(newDestination);
            }
        }

        base.Update();
    }

    private Vector3 GetRandomDestination()
    {
        float xOffset = Random.Range(-enemy.wanderRadius, enemy.wanderRadius);
        float yOffset = Random.Range(-enemy.wanderRadius, enemy.wanderRadius);
        Vector3 newDestination =
            new Vector3(enemy.transform.position.x + xOffset, enemy.transform.position.y + yOffset);
        return newDestination;
    }
}
