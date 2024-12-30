using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandController_Enemy : MonoBehaviour
{
    [SerializeField] private DamageDataSerializable damageData;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private Vector2 attackCheckSize;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float yOffset;
    [SerializeField] private float attackDelay;
    [Range(0, 1)][SerializeField] private float speedMapK;

    public void Setup(Entity _owner)
    {
        damageData.SetDamageSource(_owner);
        SetPosition();
    }
    private void SetPosition()
    {
        Player player = PlayerManager.instance.player;
        float predictFactor = 1 - 1 / Mathf.Pow(Mathf.Abs(player.rg.velocity.x) + 1, speedMapK);
        float xPosition = player.transform.position.x + player.rg.velocity.x * attackDelay * predictFactor;

        float yPosition;
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, 2 * yOffset, whatIsGround);
        if(!groundCheck)
        {
            yPosition = player.transform.position.y + 0.5f * yOffset;
        }
        else
        {
            yPosition = groundCheck.point.y + yOffset;
        }

        transform.position = new Vector3(xPosition, yPosition);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackCheck.position, attackCheckSize);
    }

    public void DamageTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(attackCheck.position, attackCheckSize, 0, whatIsPlayer);
        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Player>().cs.TakeDamage(damageData.GetDamageData(), transform);
                return;
            }
        }
    }
}
