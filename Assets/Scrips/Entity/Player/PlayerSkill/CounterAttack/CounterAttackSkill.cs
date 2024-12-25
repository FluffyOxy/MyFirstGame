using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterAttackSkill : Skill
{
    [Header("counterAttack")]
    [SerializeField] public bool isUnlocked_counterAttack;
    [SerializeField] UI_SkillSlot unlockButton_counterAttack;

    [Header("counterAttackRecover")]
    [SerializeField] public bool isUnlocked_counterAttackRecover;
    [SerializeField] UI_SkillSlot unlockButton_counterAttackRecover;
    [SerializeField] float recoverHealthPercentage = 0.1f;

    [Header("counterAttackCreateClone")]
    [SerializeField] public bool isUnlocked_counterAttackCreateClone;
    [SerializeField] UI_SkillSlot unlockButton_counterAttackCreateClone;
    [SerializeField] private float counterAttackMirrorClone_Offset = 2f;
    [SerializeField] private float counterAttackMirrorClone_Delay = 0.5f;

    protected override void Start()
    {
        base.Start();

        unlockButton_counterAttack.GetComponent<Button>().onClick.AddListener(Unlock_counterAttack);
        unlockButton_counterAttackRecover.GetComponent<Button>().onClick.AddListener(Unlock_counterAttackRecover);
        unlockButton_counterAttackCreateClone.GetComponent<Button>().onClick.AddListener(Unlock_counterAttackCreateClone);
    }

    private void Unlock_counterAttack()
    {
        if (unlockButton_counterAttack.isUnlocked)
        {
            isUnlocked_counterAttack = true;
        }
    }
    private void Unlock_counterAttackRecover()
    {
        if (unlockButton_counterAttackRecover.isUnlocked)
        {
            isUnlocked_counterAttackRecover = true;
        }
    }
    private void Unlock_counterAttackCreateClone()
    {
        if (unlockButton_counterAttackCreateClone.isUnlocked)
        {
            isUnlocked_counterAttackCreateClone = true;
        }
    }

    public bool TryCreateClone_CounterAttack()
    {
        if (isUnlocked_counterAttackCreateClone)
        {
            Invoke("CreateClone_CounterAttack_Helper", counterAttackMirrorClone_Delay);
            return true;
        }
        return false;
    }
    private void CreateClone_CounterAttack_Helper()
    {
        SkillManager.intance.clone.CreateClone(player.transform, new Vector3(player.facingDir * counterAttackMirrorClone_Offset, 0, 0));
    }

    public bool TryRecoverHealth_CounterAttack()
    {
        if(isUnlocked_counterAttackRecover)
        {
            PlayerManager.instance.player.cs.Heal(PlayerManager.instance.player.cs.GetStatByType(StatType.MaxHealth) * recoverHealthPercentage);
            return true;
        }
        return false;
    }
}
