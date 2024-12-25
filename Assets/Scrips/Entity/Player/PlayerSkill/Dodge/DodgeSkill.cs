using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [Header("Dodge")]
    [SerializeField] public bool isUnlocked_dodge;
    [SerializeField] public UI_SkillSlot unlockButton_dodge;
    [Range(0,100)][SerializeField] private float dodgeRate;

    [Header("Dodge Clone")]
    [SerializeField] public bool isUnlocked_dodgeClone;
    [SerializeField] public UI_SkillSlot unlockButton_dodgeClone;
    [SerializeField] private float dodgeClone_Offset = 2f;
    [SerializeField] private float dodgeClone_Delay = 0.5f;
    private Enemy attackSource;

    protected override void Start()
    {
        base.Start();
        unlockButton_dodge.GetComponent<Button>().onClick.AddListener(() => { isUnlocked_dodge = unlockButton_dodge.isUnlocked; });
        unlockButton_dodgeClone.GetComponent<Button>().onClick.AddListener(() => { isUnlocked_dodgeClone = unlockButton_dodgeClone.isUnlocked; });
    }

    public bool CanDodge()
    {
        if(isUnlocked_dodge)
        {
            return Random.Range(0f, 100f) < dodgeRate;
        }
        else
        {
            return false;
        }
    }

    public void SetAttackSource(Enemy _attackSource)
    {
        attackSource = _attackSource;
    }

    public override void UseSkill()
    {
        base.UseSkill();
        if(isUnlocked_dodgeClone)
        {
            Invoke("DodgeClone_Helper", dodgeClone_Delay);
        }
    }
    private void DodgeClone_Helper()
    {
        if(attackSource != null)
        {
            SkillManager.intance.clone.CreateClone(attackSource.transform, new Vector3(-attackSource.facingDir * dodgeClone_Offset, 0, 0));
            attackSource = null;
        }
    }
}
