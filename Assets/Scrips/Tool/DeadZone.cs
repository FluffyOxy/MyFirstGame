using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] private DamageDataSerializable damageData;
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if(_collision.GetComponent<Enemy>() != null)
        {
            _collision.GetComponent<Enemy>().Die();
        }
        else if (_collision.GetComponent<Player>() != null)
        {
            _collision.GetComponent<Player>().cs.TakeDamage(damageData.GetDamageData(), transform);

            CheckPoint nearestCheckPoint = GameManager.instance.TryGetClosestCheckPointToPlayer();
            if(nearestCheckPoint != null)
            {
                _collision.GetComponent<Player>().transform.position = nearestCheckPoint.transform.position;
            }
            else
            {
                _collision.GetComponent<Player>().Die();
            }
        }
    }
}
