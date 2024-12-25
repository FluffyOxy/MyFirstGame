using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SwordType
{
    Normal,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skill
{
    [Header("SwordSkill Info")]
    [SerializeField] public bool isUnlocked_sword;
    [SerializeField] private SwordType swordType;
    [SerializeField] private UI_SkillSlot unlockButton_sword;
    [SerializeField] private UI_SkillSlot unlockButton_bounce;
    [SerializeField] private UI_SkillSlot unlockButton_pierce;
    [SerializeField] private UI_SkillSlot unlockButton_spin;
    [Space]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchSpeed;
    [SerializeField] private float swordGravity;
    [SerializeField] private float returnSpeed;
    [SerializeField] public float catchFeedback;
    [SerializeField] private float swordCatchDistance;
    [SerializeField] private float minDamageRate = 0.8f;
    [SerializeField] private float enhenceDamageRate = 1.2f;

    [Header("Sword Time Stop")]
    [SerializeField] private bool isUnlocked_swordTimeStop;
    [SerializeField] private UI_SkillSlot unlockButton_swordTimeStop;
    [SerializeField] private float swordHitFreezeEnemyTime;
    [Header("Sword Enhencemnet")]
    [SerializeField] private bool isUnlocked_swordEnhencemnet;
    [SerializeField] private UI_SkillSlot unlockButton_swordEnhencemnet;

    [Header("BounceSword Info")]
    [SerializeField] private int bounceTime;
    [SerializeField] private float bouncingRadius;
    [SerializeField] private float bouncingSpeed;
    [SerializeField] private float bouncingGravity;

    [Header("PierceSword Info")]
    [SerializeField] private int pierceTime;
    [SerializeField] private float pierceGravity;

    [Header("SpinSword Info")]
    [SerializeField] private float maxTravelDistance;
    [SerializeField] private float spinDuration;
    [SerializeField] private float spinGravity;
    [SerializeField] private float spinDamageCooldown;
    [SerializeField] private float spinMoveForwardSpeed;
    [SerializeField] private float spinMoveForwardDistance;

    [Header("Aimming Line")]
    [SerializeField] private int dotNum;
    [SerializeField] private float betweenDotSpace;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;
    private GameObject[] dots;

    private Vector2 finalDir;

    protected override void Start()
    {
        base.Start();
        generateDots();
        SetDotsActive(false);

        unlockButton_sword.GetComponent<Button>().onClick.AddListener(() => { isUnlocked_sword = unlockButton_sword.isUnlocked; });
        unlockButton_bounce.GetComponent<Button>().onClick.AddListener(() => { if (unlockButton_bounce.isUnlocked) { swordType = SwordType.Bounce; } });
        unlockButton_pierce.GetComponent<Button>().onClick.AddListener(() => { if (unlockButton_pierce.isUnlocked) { swordType = SwordType.Pierce; } });
        unlockButton_spin.GetComponent<Button>().onClick.AddListener(() => { if (unlockButton_spin.isUnlocked) { swordType = SwordType.Spin; } });
        unlockButton_swordTimeStop.GetComponent<Button>().onClick.AddListener(() => { isUnlocked_swordTimeStop = unlockButton_swordTimeStop.isUnlocked; });
        unlockButton_swordEnhencemnet.GetComponent<Button>().onClick.AddListener(() => { isUnlocked_swordEnhencemnet = unlockButton_swordEnhencemnet.isUnlocked; });
    }

    protected override void Update()
    {
        base.Update();
        SetUpGravity();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchSpeed.x, AimDirection().normalized.y * launchSpeed.y);
        }
        if(Input.GetKey(KeyCode.Mouse1))
        {
            for(int i = 0; i < dots.Length; ++i)
            {
                dots[i].transform.position = GetPositionByTime(i * betweenDotSpace);
            }
        }
    }

    public void CreateSword()
    {
        SetCoolDown();
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, player.transform.rotation);
        player.AssignThrownSword(newSword);

        if (swordType == SwordType.Bounce)
        {
            newSword.GetComponent<SwordSkillController>().SetUpBounce(bounceTime, bouncingRadius, bouncingSpeed);
        }
        else if (swordType == SwordType.Pierce)
        {
            newSword.GetComponent<SwordSkillController>().SetUpPierce(pierceTime);
        }
        else if(swordType == SwordType.Spin)
        {
            newSword.GetComponent<SwordSkillController>().SetUpSpin(maxTravelDistance, spinDuration, spinDamageCooldown, spinMoveForwardSpeed, player.facingDir * spinMoveForwardDistance);
        }

        float swordFreezeTime = 0;
        if(isUnlocked_swordTimeStop)
        {
            swordFreezeTime = swordHitFreezeEnemyTime;
        }
        float damageRate = minDamageRate;
        if(isUnlocked_swordEnhencemnet)
        {
            damageRate = enhenceDamageRate;
        }
        newSword.GetComponent<SwordSkillController>().SetUp(finalDir, swordGravity, returnSpeed, swordCatchDistance, swordFreezeTime, damageRate);
        SetDotsActive(false);

    }

    private void SetUpGravity()
    {
        if (swordType == SwordType.Bounce)
        {
            swordGravity = bouncingGravity;
        }
        else if (swordType == SwordType.Pierce)
        {
            swordGravity = pierceGravity;
        }
        else if(swordType == SwordType.Spin)
        {
            swordGravity = spinGravity;
        }
    }
    public Vector2 AimDirection()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePos - playerPos;
    }

    public void SetDotsActive(bool _isActive)
    {
        for(int i = 0; i < dotNum; ++i)
        {
            dots[i].SetActive(_isActive);
        }
    }

    public void generateDots()
    {
        dots = new GameObject[dotNum];
        for(int i = 0; i < dotNum; ++i)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
        }
    }

    public Vector2 GetPositionByTime(float _time)
    {
        Vector2 position = 
            (Vector2)player.transform.position
            + new Vector2(AimDirection().normalized.x * launchSpeed.x, AimDirection().normalized.y * launchSpeed.y)
            * _time + 0.5f * (Physics2D.gravity * swordGravity) * (_time * _time);
        return position;
    }
}
