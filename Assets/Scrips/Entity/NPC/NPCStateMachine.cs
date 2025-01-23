using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{
    public NPCState currentState { get; private set; }
    public NPC npc { get; private set; }

    public NPCStateMachine(NPC _npc)
    {
        npc = _npc;
    }

    public void Initialize(NPCState _state)
    {
        currentState = _state;
        currentState.Enter();
    }

    public void changeState(NPCState _state)
    {
        currentState.Exit();
        currentState = _state;
        currentState.Enter();
    }
}
