using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Time Frozen Low HP Effect", menuName = "Item Effect/Time Frozen Low HP")]
public class TimeFrozenLowHP_Effect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float LowHPPercentage;
    [SerializeField] private float coolDownDuration;
    [SerializeField] private float timeFrozenRadius;
    [SerializeField] private float timeFrozenDuration;

    private float lastUseTime;

    private void OnEnable()
    {
        lastUseTime = -coolDownDuration;
    }

    public override void ExcuteEffect(EffectExcuteData _target)
    {
        Player player = PlayerManager.instance.player;
        if (player.cs.getCurrentHealthValue() / player.cs.getMaxHealthValue() < LowHPPercentage && !IsInCoolDown())
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, timeFrozenRadius);
            foreach(Collider2D hit in colliders)
            {
                if(hit.GetComponent<Enemy>() != null)
                {
                    hit.GetComponent<Enemy>().StartCoroutine(hit.GetComponent<Enemy>().FreezeTimerFor(timeFrozenDuration));
                }
            }
            lastUseTime = Time.time;
        }
    }

    public bool IsInCoolDown()
    {
        return Time.time < lastUseTime + coolDownDuration;
    }
}
