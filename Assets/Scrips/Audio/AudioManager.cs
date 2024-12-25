using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceEffectHelper
{
    public float pitch;
    public float volume;
    public AudioSourceEffectHelper(float pitch, float volume)
    {
        this.pitch = pitch;
        this.volume = volume;
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinDistance;
    [SerializeField] private AudioSource[] sfxArray;
    private List<AudioSourceEffectHelper> sfxEffectHelpers = new List<AudioSourceEffectHelper>();
    [SerializeField] private AudioSource[] bgmArray;

    private Dictionary<int, Coroutine> sfxDecreaseCoroutine = new Dictionary<int, Coroutine>();

    public bool isPlayBGM;
    [SerializeField] private int bgmIndex;

    [SerializeField] private Vector2 pitchRandomRange;

    public bool canPlaySFX = false;
    [SerializeField] private float allowSFXTime = 1f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }

        Invoke("AllowSFX", allowSFXTime);

        foreach(var sfx in sfxArray)
        {
            sfxEffectHelpers.Add(new AudioSourceEffectHelper(sfx.pitch, sfx.volume));
        }
    }

    private void Update()
    {
        if(!isPlayBGM)
        {
            StopAllBGM();
        }
        else if (!bgmArray[bgmIndex].isPlaying)
        {
            PlayBGM(bgmIndex);
        }
    }

    public void PlayRandomBGMInRange(int _begin, int _end)
    {
        int randomIndex = Random.Range(_begin, _end);
        PlayBGM(randomIndex);
    }

    public void PlaySFX(int _sfxIndex, Transform _sfxSourceTransform, bool isRandomPitch)
    {
        if(!canPlaySFX)
        {
            return;
        }

        if(_sfxSourceTransform != null && Vector2.Distance(_sfxSourceTransform.position, PlayerManager.instance.player.transform.position) > sfxMinDistance)
        {
            return;
        }

        if(_sfxIndex < sfxArray.Length)
        {
            if (sfxDecreaseCoroutine.TryGetValue(_sfxIndex, out Coroutine value))
            {
                StopCoroutine(value);
                sfxArray[_sfxIndex].volume = sfxEffectHelpers[_sfxIndex].volume;
                sfxDecreaseCoroutine.Remove(_sfxIndex);
            }
            if (!sfxArray[_sfxIndex].isPlaying)
            {
                if (isRandomPitch)
                {
                    sfxArray[_sfxIndex].pitch = Random.Range(pitchRandomRange.x, pitchRandomRange.y);
                }
                sfxArray[_sfxIndex].Play();
            }
        }
    }

    public void StopSFXWithTime(int _index, float duration = 1f, float _smooth = 0.1f)
    { 
        if(sfxArray[_index] != null && !sfxDecreaseCoroutine.ContainsKey(_index))
        {
            sfxDecreaseCoroutine.Add(_index, StartCoroutine(DecreaseVolume(_index, duration, _smooth)));
        }
    }

    private IEnumerator DecreaseVolume(int _index, float _time, float _smooth)
    {
        float decreaseSpeed = _smooth / _time;
        AudioSource audio = sfxArray[_index];
        float defaultVolume = audio.volume;
        while (audio.volume > 0.1f)
        {
            audio.volume -= audio.volume * decreaseSpeed;
            yield return new WaitForSeconds(_smooth);

            if(audio.volume < 0.1f)
            {
                audio.Stop();
                audio.volume = defaultVolume;
                sfxDecreaseCoroutine.Remove(_index);
                break;
            }
        }
    }

    public void StopSFX(int _sfxIndex)
    {
        if (_sfxIndex < sfxArray.Length)
        {
            sfxArray[_sfxIndex].Stop();
        }
    }

    public void PlayBGM(int _bgmIndex)
    {
        if(_bgmIndex < bgmArray.Length)
        {
            bgmIndex = _bgmIndex;
            StopAllBGM();
            bgmArray[_bgmIndex].Play();
        }
    }

    public void StopAllBGM()
    {
        foreach (var bgm in bgmArray)
        {
            bgm.Stop();
        }
    }

    private void AllowSFX()
    {
        canPlaySFX = true;
    }

    public void PlayerAttack(int _attckCount)
    {
        StopSFX(_attckCount);
        PlaySFX(_attckCount, null, true);
    }

    public void SkeletonExitIdle(Transform _skeletonTransform)
    {
        PlaySFX(4, _skeletonTransform, true);
    }

    public void PlayerMove(GroundType _groundType)
    {
        if(_groundType == GroundType.Rock)
        {
            PlaySFX(6, null, false);
        }
    }
    public void PlayerStopMove(GroundType _groundType)
    {
        if (_groundType == GroundType.Rock)
        {
            StopSFX(6);
        }
    }

    public void PlayerJump()
    {
        PlaySFX(8, null, true);
    }

    public void PlayerDash()
    {
        PlaySFX(8, null, true);
    }

    public void SkeletonHit()
    {

    }

    public void SkeletonAttack()
    {

    }
}

public enum GroundType
{
    Grass,
    Rock,
    Wood,
    Water
}