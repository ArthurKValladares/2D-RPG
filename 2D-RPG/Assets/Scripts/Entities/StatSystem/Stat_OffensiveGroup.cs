using UnityEngine;
using System;

public enum ElementalDamageType
{
    None = 0,
    Fire,
    Ice,
    Lightning
};

[Serializable]
public class Stat_OffensiveGroup
{
    public Stat<float> physicalDamage;
    public Stat<float> critPower;
    public Stat<float> critChance;
    public Stat<float> armorReduction;

    public Stat<float> fireDamage;
    public Stat<float> iceDamage;
    public Stat<float> lightningDamage;
}
