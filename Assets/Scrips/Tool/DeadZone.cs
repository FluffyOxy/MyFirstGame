using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] private DamageDataSerializable damageData;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().Die();
        }
        else if (collision.GetComponent<Player>() != null)
        {
            collision.GetComponent<Player>().cs.TakeDamage(damageData.GetDamageData(), transform);

            CheckPoint nearestCheckPoint = GameManager.instance.TryGetClosestCheckPointToPlayer();
            if(nearestCheckPoint != null)
            {
                collision.GetComponent<Player>().transform.position = nearestCheckPoint.transform.position;
            }
            else
            {
                collision.GetComponent<Player>().Die();
            }
        }
    }
}
