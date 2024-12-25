using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")]
    [SerializeField] public bool isUnlocked_dash;
    [SerializeField] private UI_SkillSlot unlockButton_dash;

    [Header("Clone on dash")]
    [SerializeField] public bool isUnlocked_cloneOnDash;
    [SerializeField] private UI_SkillSlot unlockButton_cloneOnDash;

    [Header("Clone on arrival")]
    [SerializeField] public bool isUnlocked_cloneOnArrival;
    [SerializeField] private UI_SkillSlot unlockButton_cloneOnArrival;

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();

        unlockButton_dash.GetComponent<Button>().onClick.AddListener(Unlock_dash);
        unlockButton_cloneOnDash.GetComponent<Button>().onClick.AddListener(Unlock_cloneOnDash);
        unlockButton_cloneOnArrival.GetComponent<Button>().onClick.AddListener(Unlock_cloneOnArrival);
    }

    private void Unlock_dash()
    {
        if (unlockButton_dash.isUnlocked)
        {
            isUnlocked_dash = true;
        }
    }

    private void Unlock_cloneOnDash()
    {
        if(unlockButton_cloneOnDash.isUnlocked)
        {
            isUnlocked_cloneOnDash = true;
        }
    }

    private void Unlock_cloneOnArrival()
    {
        if(unlockButton_cloneOnArrival.isUnlocked)
        {
            isUnlocked_cloneOnArrival = true;
        }
    }

    public bool TryCreateClone_DashStart()
    {
        if (isUnlocked_cloneOnDash)
        {
            SkillManager.intance.clone.CreateClone(player.transform, Vector3.zero);
            return true;
        }
        return false;
    }

    public bool TryCreateClone_DashEnd()
    {
        if (isUnlocked_cloneOnArrival)
        {
            SkillManager.intance.clone.CreateClone(player.transform, Vector3.zero);
            return true;
        }
        return false;
    }
}
