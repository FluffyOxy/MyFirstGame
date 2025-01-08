using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour, ISaveManager
{
    public Slider slider;
    public string paramter;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier;

    public void LoadData(GameData _data)
    {
        if(paramter == "bgm")
        {
            slider.value = _data.bgmVolume;
            SliderValue(_data.bgmVolume);
        }
        else if(paramter == "sfx")
        {
            slider.value = _data.sfxVolume;
            SliderValue(_data.sfxVolume);
        }
        else if (paramter == "env")
        {
            slider.value = _data.envVolume;
            SliderValue(_data.envVolume);
        }
    }

    public void SaveData(ref GameData _data)
    {
        if (paramter == "bgm")
        {
            _data.bgmVolume = slider.value;
        }
        else if (paramter == "sfx")
        {
            _data.sfxVolume = slider.value;
        }
        else if (paramter == "env")
        {
            _data.envVolume = slider.value;
        }
    }

    public void SliderValue(float _value)
    {
        audioMixer.SetFloat(paramter, (1 + Mathf.Log(_value + 0.00000001f)) * multiplier);
    }
}
