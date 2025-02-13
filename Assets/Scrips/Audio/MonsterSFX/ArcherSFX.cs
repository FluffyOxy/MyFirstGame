using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSFX : SoundManagerBase
{
    [Header("Sounds")]
    [SerializeField] protected AudioSource attack_SS;

    public Sound attack { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        attack = GetSound(attack_SS, true);
    }
}
