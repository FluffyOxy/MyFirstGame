using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;

    [Header("Clone Attack Info")]
    [SerializeField] public bool isUnlocked_cloneAttack;
    [SerializeField] private UI_SkillSlot unlockButton_cloneAttack;
    [SerializeField] public bool isUnlocked_cloneAttackEnhance;
    [SerializeField] private UI_SkillSlot unlockButton_cloneAttackEnhance;
    [SerializeField] private float cloneDamageRate = 0.2f;
    [SerializeField] private float cloneDamageRateEnhance = 0.8f;

    [Header("Crystal Instead Of Clone Info")]
    [SerializeField] public bool isUnlocked_crystalInsteadOfClone;
    [SerializeField] private UI_SkillSlot unlockButton_crystalInsteadOfClone;

    [Header("Multi Clone")]
    [SerializeField] public bool isUnlocked_multiClone;
    [SerializeField] private UI_SkillSlot unlockButton_multiClone;
    [Range(0, 1)][SerializeField] private float duplicateCloneRate;

    [Header("Test Info")]
    [SerializeField] private float duplicateCloneOffsetToClosestEnemy = 1f;

    protected override void Start()
    {
        base.Start();
        unlockButton_cloneAttack.GetComponent<Button>().onClick.AddListener(() => { isUnlocked_cloneAttack = unlockButton_cloneAttack.isUnlocked; });
        unlockButton_cloneAttackEnhance.GetComponent<Button>().onClick.AddListener(() => { isUnlocked_cloneAttackEnhance = unlockButton_cloneAttackEnhance.isUnlocked; });
        unlockButton_crystalInsteadOfClone.GetComponent<Button>().onClick.AddListener(() => { isUnlocked_crystalInsteadOfClone = unlockButton_crystalInsteadOfClone.isUnlocked; });
        unlockButton_multiClone.GetComponent<Button>().onClick.AddListener(() => { isUnlocked_multiClone = unlockButton_multiClone.isUnlocked; });
    }


    //If _targetEnemy == null, then target the closest enemy.
    public void CreateClone(Transform _clonePosition, Vector3 _offset, Enemy _targetEnemy = null)
    {
        if(isUnlocked_crystalInsteadOfClone)
        {
            if(_targetEnemy == null)
            {
                SkillManager.intance.crystal.CreateCrystalTargetClosestEnemy(_clonePosition);
            }
            else
            {
                SkillManager.intance.crystal.CreateCrystalTargetToEnemy(_targetEnemy, _clonePosition);
            }
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);
        Enemy targetEnemy = _targetEnemy;
        if (targetEnemy == null)
        {
            targetEnemy = TryFindClosestEnemy(_clonePosition);
        }

        float damageRate = cloneDamageRate;
        if(isUnlocked_cloneAttackEnhance)
        {
            damageRate = cloneDamageRateEnhance;
        }

        newClone.GetComponent<CloneSkillController>().SetupClone(
            _clonePosition, 
            cloneDuration, 
            isUnlocked_cloneAttack, 
            _offset, 
            targetEnemy, 
            isUnlocked_multiClone, 
            duplicateCloneRate,
            duplicateCloneOffsetToClosestEnemy, 
            damageRate
        );
    }

    public bool isUsingCrystalInsteadOfClone()
    {
        return isUnlocked_crystalInsteadOfClone;
    }
}
