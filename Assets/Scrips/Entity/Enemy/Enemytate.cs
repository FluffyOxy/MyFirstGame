using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemytate
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;

    protected bool triggerCalled;
    string animBoolName;

    protected float timer;

    public Enemytate(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _enemyStateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        enemyBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimBoolName(animBoolName);
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
