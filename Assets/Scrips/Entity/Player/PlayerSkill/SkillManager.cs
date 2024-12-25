using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager intance;

    #region Skills
    public DashSkill dash { get; private set; }
    public CloneSkill clone { get; private set; }
    public SwordSkill swordThrow { get; private set; }
    public BlackHoleSkill blackHole { get; private set; }
    public CrystalSkill crystal { get; private set; }
    public CounterAttackSkill counterAttack { get; private set; }
    public DodgeSkill dodge { get; private set; }
    #endregion

    private void Awake()
    {
        if(intance != null)
        {
            Destroy(intance.gameObject);
        }
        else
        {
            intance = this;
        }

        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        swordThrow = GetComponent<SwordSkill>();
        blackHole = GetComponent<BlackHoleSkill>();
        crystal = GetComponent<CrystalSkill>();
        counterAttack = GetComponent<CounterAttackSkill>();
        dodge = GetComponent<DodgeSkill>();
    }

    private void Start()
    {

    }
}
