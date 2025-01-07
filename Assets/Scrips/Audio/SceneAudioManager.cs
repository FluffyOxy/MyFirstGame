using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceDefault
{
    public float pitch { get; private set; }
    public float volume { get; private set; }

    public AudioSourceDefault(AudioSource _source)
    {
        pitch = _source.pitch;
        volume = _source.volume;
    }
}

public class SceneAudioManager : MonoBehaviour
{
    public static SceneAudioManager instance;
    [SerializeField] public GameObject soundPrfab;
    [SerializeField] public float maxAudibleDistance;
    [SerializeField] public float pitchRandomRange;

    #region Sounds
    public BGMManager bgm { get; private set; }
    public EnvSoundManager env { get; private set; }
    public PlayerSFX playerSFX { get; private set; }
    public SkeletonSFX skeletonSFX { get; private set; }
    #endregion

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        bgm = GetComponent<BGMManager>();
        env = GetComponent<EnvSoundManager>();
        playerSFX = GetComponent<PlayerSFX>();
        skeletonSFX = GetComponent<SkeletonSFX>();
    }
}
