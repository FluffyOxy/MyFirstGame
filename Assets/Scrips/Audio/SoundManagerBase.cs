using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManagerBase : MonoBehaviour
{
    protected GameObject soundPrefab;

    protected virtual void Awake()
    {
        soundPrefab = GetComponent<SceneAudioManager>().soundPrfab;
    }
    protected virtual Sound GetSound(AudioSource _ss, bool _isRandomPitch)
    {
        Sound sound = Instantiate(soundPrefab, transform).GetComponent<Sound>();
        sound.Setup(_ss, _isRandomPitch);
        return sound;
    }
}
