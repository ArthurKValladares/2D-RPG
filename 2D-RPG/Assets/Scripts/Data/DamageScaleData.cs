using System;
using UnityEngine;

[Serializable]
public class DamageScaleData
{
    [Header("Damage")]
    public float phyiscal = 1;
    public float elemental = 1;
    public float secondaryElementMultiplier = 0.5f;

    [Header("Chill")]
    public float chillDuration = 3;
    public float chillSlowPercentage = .2f;

    [Header("Burn")]
    public float burnDuratin = 3;
    public float burnDamageScale = 1;

    [Header("Electrify")]
    public float electrifyDuration = 3;
    public float electrifyDamageScale = 1;
    public float electrifyCharge = .4f;
}
