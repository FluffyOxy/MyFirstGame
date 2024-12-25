using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private int areaSoundIndex;
    [SerializeField] private float exitDuration = 1f;

    private IEnumerator CheckPlayerInside(float _duration)
    {
        float timer = _duration;
        while(timer > 0)
        {
            Debug.Log("Check");
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size, 0);
            foreach (var collider in colliders)
            {
                if (collider.GetComponent<Player>() != null)
                {
                    Debug.Log("Find");
                    AudioManager.instance.PlaySFX(areaSoundIndex, null, false);
                    yield break;
                }
            }
            timer -= 0.5f;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            AudioManager.instance.PlaySFX(areaSoundIndex, null, false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            AudioManager.instance.StopSFXWithTime(areaSoundIndex, exitDuration);
        }
    }
}
