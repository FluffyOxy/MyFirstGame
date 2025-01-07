using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnvSoundType
{
    Wind
}
public class EnvSoundManager : SoundManagerBase
{
    #region EnvSounds
    [Header("Environment Sounds")]
    [SerializeField] private AudioSource wind_SS;

    private Sound wind;
    #endregion

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        wind = GetSound(wind_SS, false);
    }

    public Sound GetEnvSoundByType(EnvSoundType _type)
    {
        switch (_type)
        {
            case EnvSoundType.Wind: return wind;
            default: return null;
        }
    }
}
