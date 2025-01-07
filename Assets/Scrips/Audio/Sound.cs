using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource sound;
    private AudioSourceDefault soundDefalut;
    private Coroutine decreaseCoroutine;
    private bool isRandomPitch;

    public void Setup(AudioSource _envSound, bool _isRandomPitch)
    {
        sound = _envSound;
        soundDefalut = new AudioSourceDefault(_envSound);
        decreaseCoroutine = null;
        isRandomPitch = _isRandomPitch;
    }

    public void Play(Transform _soundSourceTransform)
    { 
        if (_soundSourceTransform != null)
        {
            Transform player = PlayerManager.instance.player.transform;
            if(Vector2.Distance(_soundSourceTransform.position, player.position) > SceneAudioManager.instance.maxAudibleDistance)
            {
                return;
            }
        }
        if (decreaseCoroutine != null)
        {
            StopCoroutine(decreaseCoroutine);
            decreaseCoroutine = null;
            sound.volume = soundDefalut.volume;
        }

        sound.pitch = soundDefalut.pitch;
        if(isRandomPitch)
        {
            sound.pitch = Random.Range(
                sound.pitch - SceneAudioManager.instance.pitchRandomRange,
                sound.pitch + -SceneAudioManager.instance.pitchRandomRange
            );
        }
        sound.Play();
    }

    public void Stop()
    {
        if (decreaseCoroutine != null)
        {
            StopCoroutine(decreaseCoroutine);
            decreaseCoroutine = null;
            sound.volume = soundDefalut.volume;
        }
        sound.Stop();
    }

    public void StopWithinTime(float duration = 1f, float _smooth = 0.1f)
    {
        if (sound != null && decreaseCoroutine == null)
        {
            decreaseCoroutine = StartCoroutine(DecreaseVolume(duration, _smooth));
        }
    }
    private IEnumerator DecreaseVolume(float _time, float _smooth)
    {
        float decreaseSpeed = _smooth / _time;
        while (sound.volume > 0.1f)
        {
            sound.volume -= sound.volume * decreaseSpeed;
            yield return new WaitForSeconds(_smooth);

            if (sound.volume < 0.1f)
            {
                sound.Stop();
                sound.volume = soundDefalut.volume;
                decreaseCoroutine = null;
                break;
            }
        }
    }

    public bool IsPlaying()
    {
        return sound.isPlaying;
    }
}
