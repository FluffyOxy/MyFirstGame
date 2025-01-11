using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInteractive
{
    public void Interact();
}

public class PlayerAnimation : MonoBehaviour
{
    Player player;

    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    public void AnimFinishTrigger()
    {
        player.AnimFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackValidCheck.position, player.attackValidCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                player.DoDamageTo_PrimaryAttack(hit.GetComponent<Enemy>());
            }
            hit.GetComponent<IDeflectableProjectile>()?.BeDeflected();
            hit.GetComponent<Chest>()?.Open();
            hit.GetComponent<IPlayerInteractive>()?.Interact();
        }

        SceneAudioManager.instance.playerSFX.Attack((player.primaryAttackState as PlayerPrimaryAttackState).comboCounter);
    }

    private void SwordThrowTrigger()
    {
        SkillManager.intance.swordThrow.CreateSword();
    }
}
