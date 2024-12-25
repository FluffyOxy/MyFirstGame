using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHoleSkill : Skill
{
    [Header("BlackHole Info")]
    [SerializeField] public bool isUnlocked_blackHole;
    [SerializeField] private UI_SkillSlot unlockButton_blackHole;
    [Space]
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float maxSize;
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int attacAmount;
    [SerializeField] private int cloneCreateOffset;
    [SerializeField] private float targetWindow;

    [Header("HotKey Info")]
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> hotKeyCodeList;
    [SerializeField] private float hotKeyLabelYOffset;

    [Header("Player State Info")]
    [SerializeField] public float flyTime = 0.4f;
    [SerializeField] public float flyUpSpeed = 15.0f;
    [SerializeField] public float flyDownSpeed = 0.1f;
    [SerializeField] public float delayAfterAbilityFinish = 0.5f;

    private BlackHoleController blackHole = null;

    protected override void Start()
    {
        base.Start();
        unlockButton_blackHole.GetComponent<Button>().onClick.AddListener(() => { isUnlocked_blackHole = unlockButton_blackHole.isUnlocked; });
    }

    public void createBlackHole()
    {
        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, player.transform.rotation);
        blackHole = newBlackHole.GetComponent<BlackHoleController>();
        blackHole.SetUp(growSpeed, maxSize, attackCooldown, attacAmount, cloneCreateOffset, shrinkSpeed, delayAfterAbilityFinish, targetWindow);
        blackHole.SetUpHotKey(hotKeyPrefab, hotKeyCodeList, hotKeyLabelYOffset);
    }

    public bool canPlayerExitState()
    {
        if(blackHole == null)
        {
            return true;
        }
        else
        {
            if(blackHole.canPlayerExitState)
            {
                blackHole = null;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
