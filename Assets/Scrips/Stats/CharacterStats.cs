using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public enum StatType
{
    Strength,
    Agility,
    Intelligence,
    Vitality,
    MaxHealth,
    Armor,
    Evasion,
    MagicResistance,
    Damage,
    CritChance,
    CritPower,
    FireDamage,
    IceDamage,
    LightningDamage
}
public class DamageData
{
    public Entity _damageSource { get; private set; } = null;
    public bool shouldPlayAnim { get; private set; } = true;

    public float _physical { get; private set; } = 0;
    public bool _isCrit {  get; private set; } = false;
    public float _magical { get; private set; } = 0;


    public bool _ignite { get; private set; } = false;
    public float _igniteDamageCooldown { get; private set; } = float.PositiveInfinity;
    public float _igniteDuration { get; private set; } = 0f;
    public float _igniteDamage { get; private set; } = 0f;


    public bool _chill { get; private set; } = false;
    public float _chillSlowPercentage { get; private set; } = 0f;
    public float _chillDuration { get; private set; } = 0f;
    public float _chillReduceArmorPer { get; private set; } = 20f;


    public bool _shock { get; private set; } = false;
    public float _shockDuration { get; private set; } = 0f;
    public float _thunderStrikeRadius { get; private set; } = 0f;
    public float _thunderStrikeRate { get; private set; } = 0f;
    public int _thunderStrikerCounter { get; private set; } = 0;
    public float _attackerShockReduceAccuracy { get; private set; } = 0f;



    public void SetDamageSource(Entity damageSource)
    {
        _damageSource = damageSource;
    }
    public void SetShouldPlayAnim(bool _shouldPlay)
    {
        shouldPlayAnim = _shouldPlay;
    }
    public void SetPhysicsDamage(float damage, bool isCrit)
    {
        _physical = damage;
        _isCrit = isCrit;
    }
    public void SetMagicDamage(float damage)
    {
        _magical = damage;
    }
    public void SetIgnite(float igniteDamageCooldown, float igniteDuration, float igniteDamage)
    {
        _ignite = true;
        _igniteDamageCooldown = igniteDamageCooldown;
        _igniteDuration = igniteDuration;
        _igniteDamage = igniteDamage;
    }
    public void SetChill(float chillSlowPercentage, float chillDuration, float chillReduceArmarPer)
    {
        _chill = true;
        _chillSlowPercentage = chillSlowPercentage;
        _chillDuration = chillDuration;
        _chillReduceArmorPer = chillReduceArmarPer;
    }
    public void SetShock(float shockDuration, float thunderStrikeRadius, float thunderStrikeRate, int thunderStrikerCounter, float attackerShockReduceAccuracy)
    {
        _shock = true;
        _shockDuration = shockDuration;
        _thunderStrikeRadius = thunderStrikeRadius;
        _thunderStrikeRate = thunderStrikeRate;
        _thunderStrikerCounter = thunderStrikerCounter;
        _attackerShockReduceAccuracy = attackerShockReduceAccuracy;
    }
}
[System.Serializable]
public class DamageDataSerializable
{
    public Entity _damageSource { get; private set; } = null;
    [SerializeField] public bool shouldPlayAnim = true;

    [SerializeField] private float _physical = 0;
    [SerializeField] private bool _isCrit = false;
    [SerializeField] private float _magical  = 0;


    [SerializeField] private bool _ignite = false;
    [SerializeField] private float _igniteDamageCooldown = float.PositiveInfinity;
    [SerializeField] private float _igniteDuration = 0f;
    [SerializeField] private float _igniteDamage = 0f;


    [SerializeField] private bool _chill = false;
    [SerializeField] private float _chillSlowPercentage = 0f;
    [SerializeField] private float _chillDuration = 0f;
    [SerializeField] private float _chillReduceArmor = 0f;


    [SerializeField] private bool _shock = false;
    [SerializeField] private float _shockDuration = 0f;
    [SerializeField] private float _thunderStrikeRadius = 0f;
    [SerializeField] private float _thunderStrikeRate = 0f;
    [SerializeField] private int _thunderStrikerCounter = 0;
    [SerializeField] private float _attackerShockReduceAccuracy = 0f;

    public void SetDamageSource(Entity _source)
    {
        _damageSource = _source;
    }

    public DamageData GetDamageData()
    {
        DamageData damageData = new DamageData();
        damageData.SetDamageSource(_damageSource);
        damageData.SetPhysicsDamage(_physical, _isCrit);
        damageData.SetMagicDamage(_magical);
        if(_ignite)
        {
            damageData.SetIgnite(_igniteDamageCooldown, _igniteDuration, _igniteDamage);
        }
        if(_chill)
        {
            damageData.SetChill(_chillSlowPercentage, _chillDuration, _chillReduceArmor);
        }
        if(_shock)
        {
            damageData.SetShock(_shockDuration, _thunderStrikeRadius, _thunderStrikeRate, _thunderStrikerCounter, _attackerShockReduceAccuracy);
        }
        return damageData;
    }
}

[System.Serializable]
public class StatsModifierData
{
    [Header("Major Stats")]
    public float strength;         
    public float agility;          
    public float intelligence;     
    public float vitality;         


    [Header("Defensive Stats")]
    public float maxHealth;
    public float armor;
    public float evasion;
    public float magicResistance;


    [Header("Offensive Stats")]
    public float damage;
    public float critChance;
    public float critPower;        


    [Header("Magic Stats")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    public float GetStatByType(StatType _type)
    {
        switch (_type)
        {
            case StatType.Strength: return strength;
            case StatType.Agility: return agility;
            case StatType.Intelligence: return intelligence;
            case StatType.Vitality: return vitality;
            case StatType.MaxHealth: return maxHealth;
            case StatType.Armor: return armor;
            case StatType.Evasion: return evasion;
            case StatType.MagicResistance: return magicResistance;
            case StatType.Damage: return damage;
            case StatType.CritChance: return critChance;
            case StatType.CritPower: return critPower;
            case StatType.FireDamage: return fireDamage;
            case StatType.IceDamage: return iceDamage;
            case StatType.LightningDamage: return lightningDamage;
            default: return -1;
        }
    }
}

public class CharacterStats : MonoBehaviour
{
    private Entity entity;
    private EntityFX entityFX;

    [Header("Major Stats")]
    public Stat strength;                   // 1 point => increase damage by 1 and crit.power by 1%
    public Stat agility;                    // 1 point => increase evasion by 1 and crit.chance by 1%
    public Stat intelligence;               // 1 point => increase magic damage by 1 and magic resistance by 3
    public Stat vitality;                   // 1 point => increase health by 3 points
                                            
                                            
    [Header("Defensive Stats")]             
    public Stat maxHealth;                  
    public Stat armor;                      
    public Stat evasion;                    
    public Stat magicResistance;            
                                            
                                            
    [Header("Offensive Stats")]             
    public Stat damage;                     
    public Stat critChance;                 
    public Stat critPower;                  // default value => 150%


    [Header("Magic Stats")]
    public Stat fireDamage;      
    public Stat iceDamage;       
    public Stat lightningDamage; 

    [Header("Default Info")]
    [SerializeField] protected float defaultCritPower = 150f;


    [Header("Stats Effect")]
    [SerializeField] protected float strength_damage = 1.0f;
    [SerializeField] protected float strength_critPower = 1.0f;
    [SerializeField] protected float agility_evasion = 1.0f; 
    [SerializeField] protected float agility_critChance = 1.0f;
    [SerializeField] protected float intelligence_magicDamage = 1.0f;
    [SerializeField] protected float intelligence_magicResistance = 3.0f;
    [SerializeField] protected float vitality_maxHealth = 3.0f;


    [Header("Magic Effect")]
    [SerializeField] protected float igniteDuration = 4.0f;
    [SerializeField] protected float ignite_damageCooldown = 0.3f;
    [SerializeField] protected float rateFireDamageTransformToIgniteDamage = 20f;
    protected float ignitedTimer;
    protected float igniteDamageTimer;
    [Space]
    [SerializeField] protected float chillDuration = 4.0f;
    [SerializeField] protected float chill_reduceArmorPer = 20f;
    [SerializeField] protected float chillSlowPercentage = 20f;
    protected float chillTimer;
    [Space]
    [SerializeField] protected float shockDuration = 4.0f;
    [SerializeField] protected float shock_reduceAccuracy = 20f;
    [SerializeField] protected GameObject thunderAttackPrefab;
    [SerializeField] protected float thunderStrikeRadius = 25f;
    [SerializeField] protected float thunderStrikeRate = 50f;
    [SerializeField] protected int thunderStrikerCounter = 4;
    protected float shockTimer;


    [Header("DEBUG INFO")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float igniteStateDamage = 0;
    [Space]
    [SerializeField] public bool isIgnited; // does Damage Over Time
    private float igniteDamageCooldown = float.PositiveInfinity;
    [Space]
    [SerializeField] public bool isChilled; // reduce armor 
    private float chillReduceArmorPercentage;
    [Space]
    [SerializeField] public bool isShock;   // reduce accuracy
    protected float shockReduceAccuracyPercentage;

    public System.Action onCurrentHealthChange;


    public float getMaxHealthValue()
    {
        return GetStatByType(StatType.MaxHealth);
    }

    public float getCurrentHealthValue()
    {
        return currentHealth;
    }
    protected void SetCurrentHealth(float _newCurrentHealth)
    {
        currentHealth = _newCurrentHealth;
        if (onCurrentHealthChange != null)
        {
            onCurrentHealthChange();
        }
    }


    protected virtual void Start()
    {
        entity = GetComponent<Entity>();
        entityFX = GetComponent<EntityFX>();

        critPower.SetDefaultValue(defaultCritPower);
        SetCurrentHealth(getMaxHealthValue());
    }

    protected virtual void Update()
    {
        if (isIgnited)
        {
            ignitedTimer -= Time.deltaTime;
            igniteDamageTimer -= Time.deltaTime;

            if (ignitedTimer < 0)
            {
                isIgnited = false;
                igniteDamageCooldown = float.PositiveInfinity;
            }

            if (igniteDamageTimer < 0)
            {
                igniteDamageTimer = igniteDamageCooldown;
                SetCurrentHealth(currentHealth - igniteStateDamage);
                if(currentHealth < 0)
                {
                    entity.Die();
                }
            }
        }

        if (isChilled)
        {
            chillTimer -= Time.deltaTime;
            if (chillTimer < 0)
            {
                isChilled = false;
                chillReduceArmorPercentage = 0;
            }
        }

        if (isShock)
        {
            shockTimer -= Time.deltaTime;
            if (shockTimer < 0)
            {
                isShock = false;
                shockReduceAccuracyPercentage = 0;
            }
        }
    }

    public virtual float DoDamageTo_PrimaryAttack(Entity _target)
    {
        DamageData damageData = new DamageData();
        damageData.SetDamageSource(entity);
        damageData.SetShouldPlayAnim(true);
        CalculatePhysicalDamage(damageData);
        CalculateCritDamage(damageData);
        CalculateMagicDamage(damageData);
        CalculateAilment(damageData);

        _target.DamageSourceNotice(entity);

        if (UnityEngine.Random.Range(0, 100) < (1 - shockReduceAccuracyPercentage))
        {
            return 0;
        }
        return _target.GetStats().TakeDamage(damageData, transform);
    }
    protected virtual void CalculatePhysicalDamage(DamageData _damageData)
    {
        _damageData.SetPhysicsDamage(GetStatByType(StatType.Damage), false);
    }
    protected virtual void CalculateCritDamage(DamageData _damageData)
    {
        float totalCritChance = GetStatByType(StatType.CritChance);
        if (UnityEngine.Random.Range(0, 100) < totalCritChance)
        {
            _damageData.SetPhysicsDamage(_damageData._physical * GetStatByType(StatType.CritPower) * 0.01f, true);
        }
    }
    protected virtual void CalculateMagicDamage(DamageData _damageData)
    {
        _damageData.SetMagicDamage(
            Mathf.Max(GetStatByType(StatType.FireDamage), GetStatByType(StatType.IceDamage), GetStatByType(StatType.LightningDamage))
        );
    }
    protected virtual void CalculateAilment(DamageData _damageData)
    {
        float maxMagicDamage = Mathf.Max(fireDamage.GetValue(), iceDamage.GetValue(), lightningDamage.GetValue());

        if (maxMagicDamage == 0)
        {
            return;
        }

        bool ignite = fireDamage.GetValue() == maxMagicDamage;
        bool chill = iceDamage.GetValue() == maxMagicDamage;
        bool shock = lightningDamage.GetValue() == maxMagicDamage;

        int sum = 0;
        while(true)
        {
            if (ignite) ++sum;
            if (chill) ++sum;
            if (shock) ++sum;
            if (sum > 1)
            {
                int rd = UnityEngine.Random.Range(0, 3);
                if (rd == 0) ignite = false;
                else if (rd == 1) chill = false;
                else if (rd == 2) shock = false;
                sum = 0;
            }
            else
            {
                break;
            }
        }

        if (ignite)
        {
            _damageData.SetIgnite(igniteDamageCooldown, igniteDuration, rateFireDamageTransformToIgniteDamage * 0.01f * fireDamage.GetValue());
        }
        if(chill)
        {
            _damageData.SetChill(chillSlowPercentage, chillDuration, chill_reduceArmorPer);
        }
        if(shock)
        {
            _damageData.SetShock(shockDuration, thunderStrikeRadius, thunderStrikeRate, thunderStrikerCounter, shock_reduceAccuracy);
        }
    }


    public virtual float TakeDamage(in DamageData _damageData, Transform _damageDirection)
    {
        float finalPhysicalDamage = CalculatePhysicalDamageTake(_damageData);
        float finalMagicalDamage = CalculateMagicalDamageTake(_damageData);

        float finalDamage = finalPhysicalDamage + finalMagicalDamage;
        if (finalDamage > 0)
        {
            SetCurrentHealth(currentHealth - finalDamage);
            entity.fx.CreatePopUpText(finalDamage.ToString());
            if(_damageData.shouldPlayAnim)
            {
                entity.DamageAnim(_damageDirection, finalDamage);
            }
            if(_damageData._isCrit)
            {
                entity.fx.CreateCritHitFX(_damageData._damageSource, entity, _damageDirection);
            }
            else
            {
                entity.fx.CreateHitFX(_damageData._damageSource, entity);
            }
            if (currentHealth <= 0)
            {
                entity.Die();
            }
        }

        if(finalMagicalDamage > 0)
        {
            ApplyAilment(_damageData);
        }

        return finalDamage;
    }
    protected virtual float CalculatePhysicalDamageTake(in DamageData _damageData)
    {
        if (TryAvoidAttack(_damageData))
        {
            return 0;
        }

        float finalDamage = DefenseDamage(_damageData._physical);

        return finalDamage;
    }
    protected virtual float CalculateMagicalDamageTake(in DamageData _damageData)
    {
        float finalMagicDamage = _damageData._magical - GetStatByType(StatType.MagicResistance);
        finalMagicDamage = Mathf.Clamp(_damageData._magical, 0.0f, float.MaxValue);

        return finalMagicDamage;
    }
    protected virtual void ApplyAilment(in DamageData _damageData)
    {
        if (isShock && _damageData._shock && UnityEngine.Random.Range(0, 100) < _damageData._thunderStrikeRate && _damageData._thunderStrikerCounter > 0)
        {
            Enemy nearestEnemy = TryFindNearestEnemyInRadius(_damageData._thunderStrikeRadius);

            if (nearestEnemy != null)
            {
                ThunderStrikeController newThunder = Instantiate(thunderAttackPrefab, transform.position, Quaternion.identity).GetComponent<ThunderStrikeController>();
                newThunder.SetUp(nearestEnemy, _damageData);
            }
        }

        //��Entity����״̬ʱ����������״̬�����Խ����޸ģ�
        if (isIgnited || isChilled || isShock)
        {
            return;
        }

        isIgnited = _damageData._ignite;
        isChilled = _damageData._chill;
        isShock = _damageData._shock;

        if(isIgnited)
        {
            igniteDamageTimer = _damageData._igniteDamageCooldown;
            igniteDamageCooldown = _damageData._igniteDamageCooldown;
            ignitedTimer = _damageData._igniteDuration;
            igniteStateDamage = _damageData._igniteDamage;
            entityFX.IgniteFX(_damageData._igniteDuration);
        }
        if (isChilled)
        {
            chillTimer = _damageData._chillDuration;
            chillReduceArmorPercentage = _damageData._chillReduceArmorPer;
            entityFX.ChillFX(_damageData._chillDuration);
            entity.slowEntityBy(_damageData._chillSlowPercentage * 0.01f, _damageData._chillDuration);
        }
        if (isShock)
        {
            shockTimer = _damageData._shockDuration;
            shockReduceAccuracyPercentage = _damageData._attackerShockReduceAccuracy;
            entityFX.ShockFX(_damageData._shockDuration);
        }
    }

    protected virtual Enemy TryFindNearestEnemyInRadius(float _radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radius);
        float minDistance = float.PositiveInfinity;
        Enemy nearestEnemy = null;
        foreach (Collider2D hit in colliders)
        {
            Enemy enemySearch = hit.GetComponent<Enemy>();
            if (enemySearch != null && enemySearch != entity)
            {
                if (Vector2.Distance(enemySearch.transform.position, entity.transform.position) < minDistance)
                {
                    nearestEnemy = enemySearch;
                }
            }
        }

        return nearestEnemy;
    }

    protected virtual float DefenseDamage(float _damage)
    {
        float armorDefence = GetStatByType(StatType.Armor);
        _damage -= armorDefence;
        _damage = Mathf.Clamp(_damage, 0.0f, float.MaxValue);
        return _damage;
    }
    protected virtual bool TryAvoidAttack(in DamageData _damageData)
    {
        float totalEvasion = GetStatByType(StatType.Evasion);
        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }

    public virtual void AddModifier(StatsModifierData _modifierData)
    {
        float currentHealthPer = currentHealth / getMaxHealthValue();

        strength.AddModifier(_modifierData.strength);
        agility.AddModifier(_modifierData.agility);
        intelligence.AddModifier(_modifierData.intelligence);
        vitality.AddModifier(_modifierData.vitality);

        damage.AddModifier(_modifierData.damage);
        critChance.AddModifier(_modifierData.critChance);
        critPower.AddModifier(_modifierData.critPower);

        maxHealth.AddModifier(_modifierData.maxHealth);
        armor.AddModifier(_modifierData.armor);
        evasion.AddModifier(_modifierData.evasion);
        magicResistance.AddModifier(_modifierData.magicResistance);

        fireDamage.AddModifier(_modifierData.fireDamage);
        iceDamage.AddModifier(_modifierData.iceDamage);
        lightningDamage.AddModifier(_modifierData.lightningDamage);

        SetCurrentHealth(currentHealthPer * getMaxHealthValue());
    }
    public virtual void RemoveModifier(StatsModifierData _modifierData)
    {
        float currentHealthPer = currentHealth / getMaxHealthValue();

        strength.RemoveModifier(_modifierData.strength);
        agility.RemoveModifier(_modifierData.agility);
        intelligence.RemoveModifier(_modifierData.intelligence);
        vitality.RemoveModifier(_modifierData.vitality);

        damage.RemoveModifier(_modifierData.damage);
        critChance.RemoveModifier(_modifierData.critChance);
        critPower.RemoveModifier(_modifierData.critPower);

        maxHealth.RemoveModifier(_modifierData.maxHealth);
        armor.RemoveModifier(_modifierData.armor);
        evasion.RemoveModifier(_modifierData.evasion);
        magicResistance.RemoveModifier(_modifierData.magicResistance);

        fireDamage.RemoveModifier(_modifierData.fireDamage);
        iceDamage.RemoveModifier(_modifierData.iceDamage);
        lightningDamage.RemoveModifier(_modifierData.lightningDamage);

        SetCurrentHealth(currentHealthPer * getMaxHealthValue());
    }

    public void Heal(float _health)
    {
        currentHealth += _health;
        if(currentHealth > getMaxHealthValue())
        {
            currentHealth = getMaxHealthValue();
        }
        if (onCurrentHealthChange != null)
        {
            onCurrentHealthChange();
        }
    }

    public IEnumerator ModifyStatsInDurationCoroutine(StatsModifierData _modifierData, float buffDuration)
    {
        PlayerManager.instance.player.cs.AddModifier(_modifierData);
        yield return new WaitForSeconds(buffDuration);
        PlayerManager.instance.player.cs.RemoveModifier(_modifierData);
    }

    public float GetStatByType(StatType _type) 
    {
        switch (_type)
        {
            case StatType.Strength        : return strength.GetValue();
            case StatType.Agility         : return agility.GetValue();
            case StatType.Intelligence    : return intelligence.GetValue();
            case StatType.Vitality        : return vitality.GetValue();

            case StatType.MaxHealth       : return maxHealth.GetValue() + vitality.GetValue() * vitality_maxHealth;
            case StatType.Armor           : return armor.GetValue() * (1 - chillReduceArmorPercentage * 0.01f);
            case StatType.Evasion         : return evasion.GetValue() + agility.GetValue() * agility_evasion;
            case StatType.MagicResistance : return intelligence.GetValue() * intelligence_magicResistance + magicResistance.GetValue();

            case StatType.Damage          : return damage.GetValue() + strength.GetValue() * strength_damage;
            case StatType.CritChance      : return critChance.GetValue() + agility.GetValue() * agility_critChance;
            case StatType.CritPower       : return critPower.GetValue() + strength.GetValue() * strength_critPower;

            case StatType.FireDamage      : return fireDamage.GetValue() + intelligence.GetValue() * intelligence_magicDamage;
            case StatType.IceDamage       : return iceDamage.GetValue() + intelligence.GetValue() * intelligence_magicDamage;
            case StatType.LightningDamage : return lightningDamage.GetValue() + intelligence.GetValue() * intelligence_magicDamage;

            default                       : Assert.IsTrue(false, "Undefine StatType!"); return -1;
        }                 
    }
}