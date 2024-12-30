using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public Enemytate currentState { get; private set; }

    public void Initialize(Enemytate state)
    {
        currentState = state;
        currentState.Enter();
    }

    public void changeState(Enemytate state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }
}
