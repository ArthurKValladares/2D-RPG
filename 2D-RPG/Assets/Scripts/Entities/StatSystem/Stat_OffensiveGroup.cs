using UnityEngine;
using System;

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
