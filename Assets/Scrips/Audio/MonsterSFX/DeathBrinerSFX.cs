using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBrinerSFX : SoundManagerBase
{
    [Header("Sounds")]
    [SerializeField] protected AudioSource attack_SS;
    [SerializeField] protected AudioSource remote_SS;
    [SerializeField] protected AudioSource flash_SS;
    [SerializeField] protected AudioSource dash_SS;
    [SerializeField] protected AudioSource roar_SS;
    [SerializeField] protected AudioSource flashAttack_SS;

    public Sound attack { get; private set; }
    public Sound remote { get; private set; }
    public Sound flash { get; private set; }
    public Sound dash { get; private set; }
    public Sound roar { get; private set; }
    public Sound flashAttack { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        attack = GetSound(attack_SS, true);
        remote = GetSound(remote_SS, true);
        flash = GetSound(flash_SS, true);
        dash = GetSound(dash_SS, true);
        roar = GetSound(roar_SS, true);
        flashAttack = GetSound(flashAttack_SS, true);
    }
}
