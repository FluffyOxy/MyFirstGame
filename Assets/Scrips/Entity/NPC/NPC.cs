using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialog
{
    public List<Sentence> sentences;
}

public class NPC : MonoBehaviour
{
    public Animator anim {  get; private set; }
    public NPCStateMachine stateMachine { get; private set; }

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        stateMachine = new NPCStateMachine(this);
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        stateMachine.currentState.Update();
    }
}
