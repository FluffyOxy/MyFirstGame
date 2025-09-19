using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private EnvSoundType type;
    [SerializeField] private float exitDuration = 1f;
    [SerializeField] public bool isActivate = true;

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (!isActivate)
        {
            return;
        }
        if (_collision.GetComponent<Player>() != null)
        {
            SceneAudioManager.instance.env.GetEnvSoundByType(type).Play(null);
        }
    }

    private void OnTriggerStay2D(Collider2D _collision)
    {
        if (isActivate)
        {
            if (_collision.GetComponent<Player>() != null && !SceneAudioManager.instance.env.GetEnvSoundByType(type).IsPlaying())
            {
                SceneAudioManager.instance.env.GetEnvSoundByType(type).Play(null);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (!isActivate)
        {
            return;
        }
        if (_collision.GetComponent<Player>() != null)
        {
            SceneAudioManager.instance.env.GetEnvSoundByType(type).StopWithinTime(exitDuration);
        }
    }
}
