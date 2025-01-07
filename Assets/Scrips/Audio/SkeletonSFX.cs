using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSFX : SoundManagerBase
{
    [Header("Sounds")]
    [SerializeField] private AudioSource moan_SS;
    public Sound moan { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        moan = GetSound(moan_SS, true);
    }
}
