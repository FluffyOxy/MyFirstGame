using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCState
{
    protected NPC npcBase;
    protected NPCStateMachine stateMachine;

    protected bool triggerCalled;
    string animBoolName;

    protected float timer;

    public NPCState(NPC _npcBase, NPCStateMachine _npcStateMachine, string _animBoolName)
    {
        this.npcBase = _npcBase;
        this.stateMachine = _npcStateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        npcBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        npcBase.anim.SetBool(animBoolName, false);
    }

    public virtual void Update()
    {
        timer -= Time.deltaTime;
    }

    public void TriggerCall()
    {
        triggerCalled = true;
    }
}
