using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAndFireStrike_EffectController : MonoBehaviour
{
    Player player;
    DamageData damageData;
    float moveSpeed;
    float duration;

    public void SetUp(DamageData _damageData, float _moveSpeed, float _duration)
    {
        damageData = _damageData;
        moveSpeed = _moveSpeed;
        Invoke("SelfDestroy", _duration);
        SceneAudioManager.instance.itemSFX.iceAndFire.Play(null);
    }
    private void SelfDestroy()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, transform.position + transform.right, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if(_collision.GetComponent<Enemy>() != null)
        {
            _collision.GetComponent<Enemy>().cs.TakeDamage(damageData, transform);
        }
    }
}
