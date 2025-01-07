using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMType
{
    Normal,
    Fight
}
public class BGMManager : SoundManagerBase
{
    [SerializeField] private AudioSource[] normalBGMList;
    [SerializeField] private AudioSource[] fightBGMList;

    [SerializeField] public bool isPlayBGM;
    [SerializeField] public BGMType type;
    [SerializeField] private int bgmIndex;

    private void Update()
    {
        if (!isPlayBGM)
        {
            StopAllBGM();
        }
        else if (!GetAudioSourceListByType(type)[bgmIndex].isPlaying)
        {
            PlayBGMOfType(type, bgmIndex);
        }
    }

    private AudioSource[] GetAudioSourceListByType(BGMType _type)
    {
        switch (_type)
        {
            case BGMType.Normal: return normalBGMList;
            case BGMType.Fight:  return fightBGMList;
            default: return null;
        }
    }

    public void PlayRandomBGMOfTypeInRange(BGMType _type, int _begin, int _end)
    {
        int randomIndex = Random.Range(_begin, _end);
        PlayBGMOfType(_type, randomIndex);
    }

    public void PlayBGMOfType(BGMType _type, int _bgmIndex)
    {
        if (_bgmIndex < GetAudioSourceListByType(_type).Length)
        {
            bgmIndex = _bgmIndex;
            StopAllBGM();
            GetAudioSourceListByType(_type)[_bgmIndex].Play();
        }
    }

    public void StopAllBGM()
    {
        foreach (var bgm in normalBGMList)
        {
            bgm.Stop();
        }

        foreach (var bgm in fightBGMList)
        {
            bgm.Stop();
        }
    }
}
