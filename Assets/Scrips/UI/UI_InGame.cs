using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    SkillManager skill;

    [Header("HP")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [Header("Skill CoolDown Image")]
    [SerializeField] Image dashIcon;
    [SerializeField] Image dashCoolDown;
    [Space]
    [SerializeField] Image counterAttackIcon;
    [SerializeField] Image counterAttackCoolDown;
    [Space]
    [SerializeField] Image crystalIcon;
    [SerializeField] Image crystalCoolDown;
    [Space]
    [SerializeField] Image blackHoleIcon;
    [SerializeField] Image blackHoleCoolDown;
    [Space]
    [SerializeField] Image swordThrowIcon;
    [SerializeField] Image swordThrowCoolDown;
    [Space]
    [SerializeField] Image flaskIcon;
    [SerializeField] Image flaskCoolDown;

    [Header("Currency")]
    [SerializeField] TextMeshProUGUI currencyAmountText;
    [SerializeField] private float currencyAmount;
    [SerializeField] private float minIncreaseRate = 1000;
    [Range(1, 10)][SerializeField] private float increasedAlpha = 1.5f;

    private void Start()
    {
        playerStats.onCurrentHealthChange += UpdateHealthUI;
        skill = SkillManager.intance;

        dashIcon.gameObject.SetActive(skill.dash.isUnlocked_dash);
        counterAttackIcon.gameObject.SetActive(skill.counterAttack.isUnlocked_counterAttack);
        crystalIcon.gameObject.SetActive(true);
        blackHoleIcon.gameObject.SetActive(skill.blackHole.isUnlocked_blackHole);
        swordThrowIcon.gameObject.SetActive(skill.swordThrow.isUnlocked_sword);
        flaskIcon.gameObject.SetActive(Inventory.instance.TryGetEquipmentByType(EquipmentType.Flask) != null);
    }

    private void Update()
    {
        UpdateCurrency();

        if (skill.dash.isUnlocked_dash)
        {
            dashIcon.gameObject.SetActive(true);
            dashCoolDown.fillAmount = skill.dash.GetCooldownPercentage();
        }

        if (skill.counterAttack.isUnlocked_counterAttack)
        {
            counterAttackIcon.gameObject.SetActive(true);
            counterAttackCoolDown.fillAmount = skill.counterAttack.GetCooldownPercentage();
        }

        crystalCoolDown.fillAmount = skill.crystal.GetCooldownPercentage();

        if (skill.blackHole.isUnlocked_blackHole)
        {
            blackHoleIcon.gameObject.SetActive(true);
            blackHoleCoolDown.fillAmount = skill.blackHole.GetCooldownPercentage();
        }

        if (skill.swordThrow.isUnlocked_sword)
        {
            swordThrowIcon.gameObject.SetActive(true);
            swordThrowCoolDown.fillAmount = skill.swordThrow.GetCooldownPercentage();
        }

        if (Inventory.instance.TryGetEquipmentByType(EquipmentType.Flask) != null)
        {
            flaskIcon.gameObject.SetActive(true);
            flaskCoolDown.fillAmount = Inventory.instance.GetFlaskCooldownPercentage();
        }
    }

    private void UpdateCurrency()
    {
        if (currencyAmount < PlayerManager.instance.GetCurrencyAmount())
        {
            float increaseRate = (PlayerManager.instance.GetCurrencyAmount() - currencyAmount) / increasedAlpha;
            if(increaseRate < minIncreaseRate)
            {
                increaseRate = minIncreaseRate;
            }
            currencyAmount += increaseRate * Time.deltaTime;
        }
        else
        {
            currencyAmount = PlayerManager.instance.GetCurrencyAmount();
        }
        currencyAmountText.text = ((int)currencyAmount).ToString("#,#");
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetStatByType(StatType.MaxHealth);
        slider.value = playerStats.getCurrentHealthValue();
    }
}
