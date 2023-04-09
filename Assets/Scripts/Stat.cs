using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Stat
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public float BaseStat { get; private set; }
    [field: SerializeField] public float CalculatedStat { get; private set; }

    public void CalculateStat(List<Item> items)
    {
        float flatIncrease = 0;
        float factorIncrease = 1;
        foreach (Item item in items)
        {
            foreach (StatModifier modifier in item.modifiers)
            {
                if (modifier.StatName.Equals(Name))
                {
                    switch (modifier.Type)
                    {
                        case ModifierType.Flat:
                            flatIncrease += modifier.Value;
                            break;
                        case ModifierType.Multiplier:
                            factorIncrease *= modifier.Value;
                            break;
                    }
                }
            }
        }
        CalculatedStat = (BaseStat + flatIncrease) * factorIncrease;
    }

    public void CalculateStat() 
    {
        CalculatedStat = BaseStat;
    }
}

[Serializable]
public class TemporaryStat : Stat 
{
    public float CurrentStat;

    public void InitializeCurrent() 
    {
        CurrentStat = CalculatedStat;
    }

    public void IncreaseCurrent(float amount) 
    {
        CurrentStat += amount;
    }

    public void DecreaseCurrent(float amount) 
    {
        CurrentStat -= amount;
    }
}

[Serializable]
public class StatModifier
{
    [field: SerializeField] public string StatName { get; private set; }
    [field: SerializeField] public float Value { get; private set; }
    [field: SerializeField] public ModifierType Type { get; private set; }
}

public enum ModifierType
{
    Flat,
    Multiplier,
}