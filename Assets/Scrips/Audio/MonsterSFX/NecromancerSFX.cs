using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerSFX : SoundManagerBase
{
    [Header("Sounds")]
    [SerializeField] protected AudioSource attack_SS;
    [SerializeField] protected AudioSource skullExplode_SS;

    public Sound attack { get; private set; }
    public Sound skullExplode { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        attack = GetSound(attack_SS, true);
        skullExplode = GetSound(skullExplode_SS, true);
    }
}
