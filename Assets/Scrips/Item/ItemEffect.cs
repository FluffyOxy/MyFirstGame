using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EffectExcuteTime
{
    PrimaryAttack,
    Clone,
    Crystal,
    Sword,
    CounterAttack, 
    UseFlask,
    TakeDamage
}
public class EffectExcuteData
{
    public EffectExcuteTime excuteTime { get; private set; }
    public float damage { get; private set; }
    public Entity target { get; private set; }

    public EffectExcuteData(EffectExcuteTime _excuteTime, Entity _target = null, float _damage = 0)
    {
        excuteTime = _excuteTime;
        this.damage = _damage;
        this.target = _target;
    }
}

public class ItemEffect : ScriptableObject
{
    [SerializeField] public string description;
    public virtual void ExcuteEffect(EffectExcuteData _target)
    {

    }

    public virtual string GetDescription()
    {
        return description;
    }
}
