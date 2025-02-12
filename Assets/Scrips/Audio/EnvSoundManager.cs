using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnvSoundType
{
    Wind,
    Torch
}
public class EnvSoundManager : SoundManagerBase
{
    #region EnvSounds
    [Header("Environment Sounds")]
    [SerializeField] private AudioSource wind_SS;
    [SerializeField] private AudioSource torch_SS;

    private Sound wind;
    private Sound torch;
    #endregion

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        wind = GetSound(wind_SS, false);
        torch = GetSound(torch_SS, false);
    }

    public Sound GetEnvSoundByType(EnvSoundType _type)
    {
        switch (_type)
        {
            case EnvSoundType.Wind: return wind;
            case EnvSoundType.Torch: return torch;
            default: return null;
        }
    }
}
