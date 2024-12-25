using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;

    [Header("Level Detail")]
    [SerializeField] private int level;
    [Range(0f, 1f)][SerializeField] private float percantageModifier;

    [Header("Currency Drop")]
    public Stat currencyDropAmount;

    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>();
        ApplyLevelModifier();
    }

    private void ApplyLevelModifier()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);

        Modify(currencyDropAmount);

        SetCurrentHealth(maxHealth.GetValue());
    }

    private void Modify(Stat _stat)
    {
        for(int i = 0; i < level; i++)
        {
            float modifier = _stat.GetValue() * percantageModifier;
            _stat.AddModifier(modifier);
        }
    }
}
