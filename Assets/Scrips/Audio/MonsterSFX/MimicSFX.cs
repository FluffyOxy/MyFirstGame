using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicSFX : SoundManagerBase
{
    [Header("Sounds")]
    [SerializeField] protected AudioSource open_SS;
    [SerializeField] protected AudioSource roar_SS;
    [SerializeField] protected AudioSource attack_SS;

    public Sound open { get; private set; }
    public Sound roar { get; private set; }
    public Sound attack { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        open = GetSound(open_SS, true);
        roar = GetSound(roar_SS, true);
        attack = GetSound(attack_SS, true);
    }
}
