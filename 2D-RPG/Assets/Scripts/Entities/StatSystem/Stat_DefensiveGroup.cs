using UnityEngine;
using System;

[Serializable]
public class Stat_DefensiveGroup
{
    public Stat<float> health;

    public Stat<float> armor;
    public Stat<float> evasion;

    public Stat<float> fireResistance;
    public Stat<float> iceResistance;
    public Stat<float> lightningResistance;
}
