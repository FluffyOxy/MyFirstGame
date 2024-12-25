using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("Sword Catch FX")]
    [SerializeField] ParticleSystem swordCatchFX;

    public void SwordCatchFx()
    {
        swordCatchFX.Play();
    }
}
