using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderStateBase : NPCState
{
    protected Trader npc;

    public TraderStateBase(NPC _npcBase, NPCStateMachine _npcStateMachine, string _animBoolName, Trader _npc) : base(_npcBase, _npcStateMachine, _animBoolName)
    {
        npc = _npc;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
