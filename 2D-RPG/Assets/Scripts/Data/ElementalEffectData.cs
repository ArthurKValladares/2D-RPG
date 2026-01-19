using UnityEditor;
using UnityEngine;

public class ElementalEffectData
{
    public ElementalEffectData(Entity_Stats stats, DamageScaleData damageScale)
    {
        chillDuration = damageScale.chillDuration;
        chillSlowPercentage = damageScale.chillSlowPercentage;

        burnDuration = damageScale.burnDuratin;
        burnTotalDamage = stats.offensive.fireDamage.GetValue() * damageScale.burnDamageScale;

        electrifyDuration = damageScale.electrifyDuration;
        electrifyDamageOnFullCharge = stats.offensive.lightningDamage.GetValue() * damageScale.electrifyDamageScale;
        electrifyCharge = damageScale.electrifyCharge;
    }

    public float chillDuration;
    public float chillSlowPercentage;

    public float burnDuration;
    public float burnTotalDamage;

    public float electrifyDuration;
    public float electrifyCharge;
    public float electrifyDamageOnFullCharge;
}