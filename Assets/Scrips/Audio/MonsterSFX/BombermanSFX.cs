using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombermanSFX : SoundManagerBase
{
    [Header("Sounds")]
    [SerializeField] protected AudioSource attack_SS;
    [SerializeField] protected AudioSource explode_SS;

    public Sound attack { get; private set; }
    public Sound explode { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        attack = GetSound(attack_SS, true);
        explode = GetSound(explode_SS, true);
    }
}
