using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitcherStateBase : NPCState
{
    protected Witcher npc;
    public WitcherStateBase(NPC _npcBase, NPCStateMachine _npcStateMachine, string _animBoolName, Witcher _npc) : base(_npcBase, _npcStateMachine, _animBoolName)
    {
        this.npc = _npc;
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
