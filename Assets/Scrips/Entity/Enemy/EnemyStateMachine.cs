using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentState { get; private set; }
    public Enemy enemy { get; private set; }

    public EnemyStateMachine(Enemy _enemy)
    {
        enemy = _enemy;
    }

    public void Initialize(EnemyState state)
    {
        currentState = state;
        currentState.Enter();
    }

    public void changeState(EnemyState state)
    {
        if(enemy.isDead)
        {
            return;
        }

        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }
}
