using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerStats : CharacterStats, ISaveManager
{
    private Player player;
    private float currentFlaskUsageTime;

    [Header("Stats UI")]
    [SerializeField] private Transform statsSlotParent;
    private UI_StatsSlot[] statSLot;

    public override float TakeDamage(in DamageData _damageData, Transform _damageDirection)
    {
        if(player.isDashing)
        {
            return 0;
        }
        float getDamage = base.TakeDamage(_damageData, _damageDirection);
        EffectExcuteData data = new EffectExcuteData(EffectExcuteTime.TakeDamage, _damageData._damageSource, getDamage);
        Inventory.instance.TryGetEquipmentByType(EquipmentType.Armor)?.TryExcuteItemEffect(data);

        if(getDamage > 0)
        {
            SceneAudioManager.instance.playerSFX.playerHit.Play(null);
        }

        return getDamage;
    }

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
        statSLot = statsSlotParent.GetComponentsInChildren<UI_StatsSlot>();
        if (statSLot != null)
        {
            UpdateStatsUI();
        }
        currentFlaskUsageTime = GetStatByType(StatType.MaxFlaskUsageTime);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void RecoverFlaskUsageTimeRaw(float _amount)
    {
        currentFlaskUsageTime += _amount;
        if (currentFlaskUsageTime > GetStatByType(StatType.MaxFlaskUsageTime))
        {
            currentFlaskUsageTime = GetStatByType(StatType.MaxFlaskUsageTime);
        }

        UpdateFlaskUsageTimeInUI();
    }

    public void RecoverFlaskUsageTime(float _amount)
    {
        currentFlaskUsageTime += GetStatByType(StatType.FlaskUsageRecover) * _amount;
        if (currentFlaskUsageTime > GetStatByType(StatType.MaxFlaskUsageTime))
        {
            currentFlaskUsageTime = GetStatByType(StatType.MaxFlaskUsageTime);
        }

        UpdateFlaskUsageTimeInUI();
    }

    public void ReduceFlaskUsageTime(float _time)
    {
        currentFlaskUsageTime -= _time;
        if(currentFlaskUsageTime < 0)
        {
            currentFlaskUsageTime = 0;
        }

        UpdateFlaskUsageTimeInUI();
    }

    public bool IsFlaskUsable()
    {
        return currentFlaskUsageTime >= 1;
    }

    public int CheckFlaskUsageTimeInInt()
    {
        return (int)currentFlaskUsageTime;
    }
    private void UpdateFlaskUsageTimeInUI()
    {
        UI.instance.UpdatePlaskUsageTime(CheckFlaskUsageTimeInInt());
    }

    public override void AddModifier(StatsModifierData _modifierData)
    {
        base.AddModifier(_modifierData);
        UpdateStatsUI();
    }

    public override void RemoveModifier(StatsModifierData _modifierData)
    {
        base.RemoveModifier(_modifierData);
        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        if (statSLot != null)
        {
            foreach (var stat in statSLot)
            {
                stat.UpdateStatsSlotUI();
            }
        }
    }

    protected override bool TryAvoidAttack(in DamageData _damageData)
    {
        bool canAvoid;
        if(SkillManager.intance.dodge.CanDodge())
        {
            SkillManager.intance.dodge.SetAttackSource(_damageData._damageSource as Enemy);
            if(SkillManager.intance.dodge.TryUseSkill())
            {
                canAvoid = true;
            }
            canAvoid = false;
        }
        else
        {
            canAvoid = base.TryAvoidAttack(_damageData);
        }

        if(canAvoid)
        {
            SceneAudioManager.instance.playerSFX.evasionSuccess.Play(null);
        }

        return canAvoid;
    }

    public override float DoDamageTo_PrimaryAttack(Entity _target)
    {
        float realDamage = base.DoDamageTo_PrimaryAttack(_target);
        if(realDamage > 0)
        {
            SceneAudioManager.instance.playerSFX.swordHit.Play(null);
            RecoverFlaskUsageTime(1);
        }
        return realDamage;
    }
    public float DoDamageTo_Sword(Entity _target, float _swordDamageRate, Transform _swordTransform)
    {
        DamageData damageData = new DamageData();
        damageData.SetDamageSource(player);
        damageData.SetShouldPlayAnim(true);
        CalculatePhysicalDamage(damageData);
        CalculateCritDamage(damageData);
        CalculateMagicDamage(damageData);
        CalculateAilment(damageData);

        damageData.SetPhysicsDamage(damageData._physical * _swordDamageRate, damageData._isCrit);
        damageData.SetMagicDamage(damageData._magical * _swordDamageRate);

        _target.DamageSourceNotice(player);

        if (UnityEngine.Random.Range(0, 100) < (1 - shockReduceAccuracyPercentage))
        {
            return 0;
        }

        float realDamage = _target.GetStats().TakeDamage(damageData, _swordTransform);

        return realDamage;
    }
    public float DoDamageTo_CounterAttack(Entity _target)
    {
        DamageData damageData = new DamageData();
        damageData.SetDamageSource(player);
        damageData.SetShouldPlayAnim(false);
        CalculatePhysicalDamage(damageData);
        CalculateCritDamage(damageData);
        CalculateMagicDamage(damageData);
        CalculateAilment(damageData);

        _target.DamageSourceNotice(player);

        if (UnityEngine.Random.Range(0, 100) < (1 - shockReduceAccuracyPercentage))
        {
            return 0;
        }
        return _target.GetStats().TakeDamage(damageData, PlayerManager.instance.player.transform);
    }
    public float DoDamageTo_Crystal(Entity _target, Transform _crystalTransform)
    {
        DamageData damageData = new DamageData();
        damageData.SetDamageSource(player);
        CalculatePhysicalDamage(damageData);
        CalculateCritDamage(damageData);
        CalculateMagicDamage(damageData);
        CalculateAilment(damageData);

        _target.DamageSourceNotice(player);

        if (UnityEngine.Random.Range(0, 100) < (1 - shockReduceAccuracyPercentage))
        {
            return 0;
        }
        return _target.GetStats().TakeDamage(damageData, _crystalTransform);
    }
    public float DoDamageTo_Clone(Entity _target, float _cloneDamageRate, Transform _cloneTransform)
    {
        DamageData damageData = new DamageData();
        damageData.SetDamageSource(player);
        damageData.SetShouldPlayAnim(true);
        CalculatePhysicalDamage(damageData);
        CalculateCritDamage(damageData);
        CalculateMagicDamage(damageData);
        CalculateAilment(damageData);

        damageData.SetPhysicsDamage(damageData._physical * _cloneDamageRate, damageData._isCrit);
        damageData.SetMagicDamage(damageData._magical * _cloneDamageRate);

        _target.DamageSourceNotice(player);


        if (UnityEngine.Random.Range(0, 100) < (1 - shockReduceAccuracyPercentage))
        {
            
            return 0;
        }
        return _target.cs.TakeDamage(damageData, _cloneTransform);
    }

    public void LoadData(GameData _data)
    {
        if(_data.HP < 0)
        {
            SetCurrentHealth(GetMaxHealthValue());
        }
        else
        {
            SetCurrentHealth(_data.HP);
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.HP = GetCurrentHealthValue();
    }
}
