using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class StatModifier
{
    public StatModifier(string source, float value)
    {
        this.source = source;
        this.value = value;
    }

    public string source;
    public float value;
}

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] public List<StatModifier> modifiers = new();

    private bool wasModified = true;
    private float finalValue;

    public float GetValue() {
        if (wasModified)
        {
            finalValue = GetFinalValue();
            wasModified = false;
        }
        return finalValue;
    }

    public void SetBaseValue(float value)
    {
        baseValue = value;
    }

    public void AddModifier(string source, float value)
    {
        modifiers.Add(new StatModifier(source, value));
        wasModified = true;
    }

    public void RemoveModifier(string source)
    {
        modifiers.RemoveAll(modifier => modifier.source == source);
        wasModified = true;
    }

    private float GetFinalValue()
    {
        float value = baseValue;

        foreach (StatModifier modifier in modifiers)
        {
            value += modifier.value;
        }

        return value;
    }
}
