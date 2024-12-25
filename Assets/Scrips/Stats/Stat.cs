using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    public List<float> modifiers;

    public float GetValue() 
    {
        float finalValue = baseValue;
        foreach(float mod in modifiers)
        {
            finalValue += mod;
        }
        return finalValue; 
    }

    public void SetDefaultValue(float _value)
    {
        baseValue = _value;
    }

    public void AddModifier(float _modifier)
    {
        modifiers.Add(_modifier);
    }

    public void RemoveModifier(float _modifier)
    {
        modifiers.Remove(_modifier);
    }
}
