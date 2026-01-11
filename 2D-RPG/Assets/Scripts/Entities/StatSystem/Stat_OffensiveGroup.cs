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
    public Stat physicalDamage;
    public Stat critPower;
    public Stat critChance;
    public Stat armorReduction;

    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public Stat attackSpeedMultiplier;
}
