using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CrystalSkill : Skill
{
    [Header("Crystal Base Info")]
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;

    [Header("Crystal PlayerSwap Info")]
    [SerializeField] public bool isUnlocked_swapPlayer;
    [SerializeField] private UI_SkillSlot unlockButton_swapPlayer;
    [SerializeField] public bool isUnlocked_createCloneAtCrystalSwap;
    [SerializeField] private UI_SkillSlot unlockButton_createCloneAtCrystalSwap;

    [Header("Crystal Explode Info")]
    [SerializeField] public bool isUnlocked_explode;
    [SerializeField] private UI_SkillSlot unlockButton_explode;
    [SerializeField] private float growSpeed;
    [SerializeField] private float maxSize;

    [Header("Crystal Move Info")]
    [SerializeField] public bool isUnlocked_moveToEnemy;
    [SerializeField] private UI_SkillSlot unlockButton_moveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi Stacking Crystal Info")]
    [SerializeField] public bool isUnlocked_multiStacks;
    [SerializeField] private UI_SkillSlot unlockButton_multiStacks;
    [SerializeField] private int stackAmount;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    [Header("Crystal Clone Info")]
    [SerializeField] public bool isUnlocked_crystalClone;
    [SerializeField] private UI_SkillSlot unlockButton_crystalClone;

    [Header("Test Info")]
    [SerializeField] private float smallDistance_CrystalExpolde = 0.1f;
    [SerializeField] private float smallDistance_CrystalCreateClone = 1f;

    private GameObject currentCrystal = null;

    public override float GetCooldownPercentage()
    {
        if(isUnlocked_multiStacks)
        {
            return cooldownTimer / multiStackCooldown;
        }
        return base.GetCooldownPercentage();
    }

    public override bool TryUseSkill()
    {
        if (IsOutCooldown())
        {
            UseSkill();
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void UseSkill()
    {
        if(!tryUseMultiCrystal())
        {
            if (currentCrystal == null)
            {
                CreateCrystalTargetClosestEnemy(player.transform);
            }
            else
            {
                if (isUnlocked_swapPlayer && !isUnlocked_explode)
                {
                    Vector2 playerPos = player.transform.position;
                    player.transform.position = currentCrystal.transform.position;
                    currentCrystal.transform.position = playerPos;
                }
                currentCrystal.GetComponent<CrystalController>().FinishCrystal();
            }
        }
    }

    public void CreateCrystalTargetClosestEnemy(Transform _transform)
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, player.transform.rotation);
        currentCrystal.GetComponent<CrystalController>().
            SetUp(crystalDuration, isUnlocked_explode, growSpeed, maxSize, TryFindClosestEnemy(player.transform), isUnlocked_moveToEnemy, moveSpeed, isUnlocked_crystalClone);
        currentCrystal.GetComponent<CrystalController>().
            SetUpTestInfo(smallDistance_CrystalExpolde, smallDistance_CrystalCreateClone);
    }
    public void CreateCrystalTargetToEnemy(Enemy _enemy, Transform _transform)
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, player.transform.rotation);
        currentCrystal.GetComponent<CrystalController>().
            SetUp(crystalDuration, isUnlocked_explode, growSpeed, maxSize, _enemy, isUnlocked_moveToEnemy, moveSpeed, isUnlocked_crystalClone);
        currentCrystal.GetComponent<CrystalController>().
            SetUpTestInfo(smallDistance_CrystalExpolde, smallDistance_CrystalCreateClone);
    }

    private bool tryUseMultiCrystal()
    {
        if(isUnlocked_multiStacks)
        {
            if(crystalLeft.Count > 0)
            {
                if(crystalLeft.Count == stackAmount)
                {
                    Invoke("ResetAbility", useTimeWindow);
                }

                GameObject crytalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crytalToSpawn, player.transform.position, Quaternion.identity);
                crystalLeft.Remove(crytalToSpawn);

                newCrystal.GetComponent<CrystalController>().
                    SetUp(crystalDuration, isUnlocked_explode, growSpeed, maxSize, TryFindClosestEnemy(player.transform), isUnlocked_moveToEnemy, moveSpeed, isUnlocked_crystalClone);
                newCrystal.GetComponent<CrystalController>().
                    SetUpTestInfo(smallDistance_CrystalExpolde, smallDistance_CrystalCreateClone);

                if (crystalLeft.Count <= 0)
                {
                    cooldownTimer = multiStackCooldown;
                    CancelInvoke();
                    RefilCrystal();
                }
            }
            return true;
        }
        return false;
    }

    private void RefilCrystal()
    {
        for(int i = crystalLeft.Count; i < stackAmount; ++i) 
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    protected override void Start()
    {
        base.Start();
        RefilCrystal();

        unlockButton_createCloneAtCrystalSwap.GetComponent<Button>().onClick.AddListener(() => { isUnlocked_createCloneAtCrystalSwap = unlockButton_createCloneAtCrystalSwap.isUnlocked; });
        unlockButton_explode.GetComponent<Button>().onClick.AddListener(() => { isUnlocked_explode = unlockButton_explode.isUnlocked; });
        unlockButton_moveToEnemy.GetComponent<Button>().onClick.AddListener(() => { isUnlocked_moveToEnemy = unlockButton_moveToEnemy.isUnlocked; });
        unlockButton_multiStacks.GetComponent<Button>().onClick.AddListener(() => { isUnlocked_multiStacks = unlockButton_multiStacks.isUnlocked; });
        unlockButton_swapPlayer.GetComponent<Button>().onClick.AddListener(() => { isUnlocked_swapPlayer = unlockButton_swapPlayer.isUnlocked; });
        unlockButton_crystalClone.GetComponent<Button>().onClick.AddListener(() => { isUnlocked_crystalClone = unlockButton_crystalClone.isUnlocked; });
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }

    public bool TryCreateClone_CrystalSwap(Transform _crystal)
    {
        if (isUnlocked_createCloneAtCrystalSwap && !SkillManager.intance.clone.isUnlocked_crystalInsteadOfClone)
        {
            SkillManager.intance.clone.CreateClone(_crystal, Vector3.zero);
            return true;
        }
        return false;
    }

    public bool CanCreateClone_CrystalSwap()
    {
        return isUnlocked_createCloneAtCrystalSwap;
    }
}
