using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemSFX : SoundManagerBase
{
    [Header("Sounds")]
    [SerializeField] private AudioSource torchLighting_SS;
    [SerializeField] private AudioSource iceAndFire_SS;
    [SerializeField] private AudioSource thunderAttack_SS;
    [SerializeField] private AudioSource lightningAttack_SS;
    [SerializeField] private AudioSource heal_SS;

    public Sound torchLighting { get; private set; }
    public Sound iceAndFire { get; private set; }
    public Sound thunderAttack { get; private set; }
    public Sound lightningAttack { get; private set; }
    public Sound heal { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        torchLighting = GetSound(torchLighting_SS, true);
        iceAndFire = GetSound(iceAndFire_SS, true);
        thunderAttack = GetSound(thunderAttack_SS, true);
        lightningAttack = GetSound(lightningAttack_SS, true);
        heal = GetSound(heal_SS, true);
    }
}
