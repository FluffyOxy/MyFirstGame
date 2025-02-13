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
    [SerializeField] private AudioSource swordThrow_SS;
    [SerializeField] private AudioSource swordGround_SS;
    [SerializeField] private AudioSource swordCatch_SS;
    [SerializeField] private AudioSource counterAttack_SS;
    [SerializeField] private AudioSource counterAttackSuccess_SS;
    [SerializeField] private AudioSource blackHoleLoop_SS;
    [SerializeField] private AudioSource crystalPlace_SS;
    [SerializeField] private AudioSource crystalFlashBack_SS;
    [SerializeField] private AudioSource crystalExplode_SS;
    [SerializeField] private AudioSource evasionSuccess_SS;
    [SerializeField] private AudioSource playerHit_SS;
    [SerializeField] private AudioSource swordHit_SS;


    private List<Sound> attack = new List<Sound>();
    private Sound rockGround;
    public Sound jump { get; private set; }
    public Sound dash { get; private set; }
    public Sound swordThrow { get; private set; }
    public Sound swordGround { get; private set; }
    public Sound swordCatch { get; private set; }
    public Sound counterAttack { get; private set; }
    public Sound counterAttackSuccess { get; private set; }
    public Sound blackHoleLoop { get; private set; }
    public Sound crystalPlace { get; private set; }
    public Sound crystalFlashBack { get; private set; }
    public Sound crystalExplode { get; private set; }
    public Sound evasionSuccess { get; private set; }
    public Sound playerHit { get; private set; }
    public Sound swordHit { get; private set; }


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
        swordThrow = GetSound(swordThrow_SS, true);
        swordGround = GetSound(swordGround_SS, true);
        swordCatch = GetSound(swordCatch_SS, true);
        counterAttack = GetSound(counterAttack_SS, true);
        counterAttackSuccess = GetSound(counterAttackSuccess_SS, true);
        blackHoleLoop = GetSound(blackHoleLoop_SS, false);
        crystalPlace = GetSound(crystalPlace_SS, true);
        crystalFlashBack = GetSound(crystalFlashBack_SS, true);
        crystalExplode = GetSound(crystalExplode_SS, true);
        evasionSuccess = GetSound(evasionSuccess_SS, true);
        playerHit = GetSound(playerHit_SS, true);
        swordHit = GetSound(swordHit_SS, true);
    }

    public void Attack(int _count, Transform _source = null)
    {
        attack[_count].Stop();
        attack[_count].Play(_source);
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
