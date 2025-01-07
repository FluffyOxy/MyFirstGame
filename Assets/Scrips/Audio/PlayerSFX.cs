using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GroundType
{
    Rock
}
public class PlayerSFX : SoundManagerBase
{
    [Header("Sounds")]
    [SerializeField] private List<AudioSource> attack_SS;
    [SerializeField] private AudioSource rockGround_SS;
    [SerializeField] private AudioSource jump_SS;
    [SerializeField] private AudioSource dash_SS;

    private List<Sound> attack = new List<Sound>();
    private Sound rockGround;
    public Sound jump { get; private set; }
    public Sound dash { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        for (int i = 0; i < attack_SS.Count; ++i)
        {
            attack.Add(new Sound());
            attack[i] = GetSound(attack_SS[i], true);
        }
        rockGround = GetSound(rockGround_SS, true);
        jump = GetSound(jump_SS, true);
        dash = GetSound(dash_SS, true);
    }

    public void Attack(int _count)
    {
        attack[_count].Stop();
        attack[_count].Play(null);
    }

    public Sound GetStepSound(GroundType _ground)
    {
        switch (_ground)
        {
            case GroundType.Rock: return rockGround;
            default: return null;
        }
    }
}
