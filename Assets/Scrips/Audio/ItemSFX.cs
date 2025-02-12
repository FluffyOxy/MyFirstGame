using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemSFX : SoundManagerBase
{
    [Header("Sounds")]
    [SerializeField] private AudioSource torchLighting_SS;

    public Sound torchLighting { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        torchLighting = GetSound(torchLighting_SS, true);
    }
}
